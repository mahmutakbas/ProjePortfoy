﻿using Microsoft.Data.Analysis;
using Microsoft.ML;
using Microsoft.ML.Data;
using System.Data;
using System.Text;

namespace MLDataAccess
{
    public interface ISelectPortfoy
    {
        Task<List<ProjectPrediction>> MLPrediction(IEnumerable<ProjectGetData> itemList);
        Task<List<ProjectPrediction>> MLPredictionTest(ModelPredictionDto predictionDto);
    }

    public class SelectPortfoy : ISelectPortfoy
    {

        public Task<List<ProjectPrediction>> MLPredictionTest(ModelPredictionDto predictionDto)
        {
            var context = new MLContext();

            if (predictionDto.ItemList != null)
            {
                List<ProjectData> projects = new List<ProjectData>();
                foreach (var item in predictionDto.ItemList)
                {
                    ProjectData project = new ProjectData();

                    DateTime baslangicTarihi = item.StartDate;
                    DateTime bitisTarihi = item.FinishDate;
                    float fark = (float)Math.Round(bitisTarihi.Subtract(baslangicTarihi).TotalDays / 360);

                    project.Id = item.Id;
                    project.ProjeIsmi = "aa";
                    project.ProjeTipi = "aa";
                    project.Sure = fark;

                    project.Butce = GetButce((float)item.Budget);
                    project.TahminiKar = GetTahminKar(item.Revenue);
                    project.IsGucu = GetIsgucu((float)item.ManCount);
                    project.KaynakKullanim = GetKaynak((float)item.ResourcPercent);
                    project.Risk1 = GetRisk(item?.Risk1!.ToString());
                    project.Risk2 = GetRisk(item?.Risk2!.ToString());
                    project.Risk3 = GetRisk(item?.Risk3!.ToString());
                    project.Risk4 = GetRisk(item?.Risk4!.ToString());
                    project.Risk5 = GetRisk(item?.Risk5!.ToString());

                  //  project.Sonuc = 1; //(float)(Math.Round(project.Sure + project.Butce + project.TahminiKar + project.IsGucu + project.KaynakKullanim + ((project.Risk1 + project.Risk2 + project.Risk3 + project.Risk4 + project.Risk5) / 5) / 10));

                    projects.Add(project);
                }

                using (var stream = new FileStream("model.zip", FileMode.Open))
                {
                    // Modeli yükleme
                    var loadedModel = context.Model.Load(stream, out var modelSchema);

                    // Tahminlerde bulunma
                    var predictionEngine = context.Model.CreatePredictionEngine<ProjectData, ProjectPrediction>(loadedModel);


                    List<ProjectPrediction> predict = new List<ProjectPrediction>();

                    foreach (var inputData in projects)
                    {
                        var pred = predictionEngine.Predict(inputData);

                        predict.Add(pred);
                        Console.WriteLine($"{pred.Id} ---- {pred.Butce} ---- {pred.IsGucu} ---- {pred.KaynakKullanim} --- {pred.Sonuc}");

                    }
                   
                    //bütçe, süre, kaynak, işçi

                    var ordered = predict.OrderByDescending(p => p.Sonuc);
                    if(predictionDto.strategyName == "Bütçe")
                    {
                        ordered=ordered.ThenByDescending(p=> p.Butce);
                    }
                    else if(predictionDto.strategyName == "Kaynak")
                    {
                        ordered = ordered.ThenBy(p => p.KaynakKullanim);
                    }
                    else if(predictionDto.strategyName == "Süre")
                    {
                        ordered = ordered.ThenBy(p => p.Sure);
                    }
                    return Task.FromResult(ordered.ToList());
                }
            }

            return null;

        }



        public Task<List<ProjectPrediction>> MLPrediction(IEnumerable<ProjectGetData> itemList)
        {
            if (itemList != null)
            {
                List<ProjectData> projects = new List<ProjectData>();
                foreach (var item in itemList)
                {
                    ProjectData project = new ProjectData();

                    DateTime baslangicTarihi = item.StartDate;
                    DateTime bitisTarihi = item.FinishDate;
                    float fark = (float)Math.Round(bitisTarihi.Subtract(baslangicTarihi).TotalDays / 360);

                    project.ProjeIsmi = "aa";
                    project.Sure = fark;

                    project.Butce = GetButce((float)item.Budget);
                    project.TahminiKar = GetTahminKar(item.Revenue);
                    project.IsGucu = GetIsgucu((float)item.ManCount);
                    project.KaynakKullanim = GetKaynak((float)item.ResourcPercent);
                    project.Risk1 = GetRisk(item?.Risk1!.ToString());
                    project.Risk2 = GetRisk(item?.Risk2!.ToString());
                    project.Risk3 = GetRisk(item?.Risk3!.ToString());
                    project.Risk4 = GetRisk(item?.Risk4!.ToString());
                    project.Risk5 = GetRisk(item?.Risk5!.ToString());

                  //  project.Sonuc = (float)(Math.Round(project.Sure + project.Butce + project.TahminiKar + project.IsGucu + project.KaynakKullanim + ((project.Risk1 + project.Risk2 + project.Risk3 + project.Risk4 + project.Risk5) / 5) / 10));

                    projects.Add(project);
                }


                var context = new MLContext();

                var data = context.Data.LoadFromEnumerable<ProjectGetData>((IEnumerable<ProjectGetData>)projects);

                var trainTestData = context.Data.TrainTestSplit(data, 0.25);

                var estimator = context.Transforms.NormalizeMinMax("Label", "Sonuc")
                            .Append(context.Transforms.Categorical.OneHotEncoding("ProjeTipi"))
                            .Append(context.Transforms.Concatenate("Features", "Butce", "TahminiKar", "IsGucu", "KaynakKullanim", "Sure", "Risk1", "Risk2", "Risk3", "Risk4", "Risk5", "Sonuc", "ProjeTipi")).Append(context.Transforms.ProjectToPrincipalComponents("Features", rank: 5));

                var pcaResult = estimator.Fit(data).Transform(data);

                var trainer = context.Regression.Trainers.FastForest();

                var model1 = estimator.Append(trainer);

                var model = model1.Fit(trainTestData.TrainSet);

                var testPredictions = model.Transform(trainTestData.TestSet);

                var metrics = context.Regression.Evaluate(testPredictions);
                Console.WriteLine($"Accuracy: {metrics.LossFunction} {metrics.RSquared} {metrics.MeanSquaredError} {metrics.RootMeanSquaredError}");

                foreach (var item in testPredictions.Preview().RowView)
                {
                    Console.WriteLine($"{item.Values[0]} --- {item.Values[8]} --- {item.Values[18]}");
                }

                var predictionData = context.Data.CreateEnumerable<ProjectPrediction>(testPredictions, reuseRowObject: false).ToList();

                var newPre = predictionData.OrderByDescending(p => p.Sonuc).ToList();

                foreach (var item in newPre)
                {
                    Console.WriteLine($"Proje : {item.ProjeIsmi} \tBütçe : {item.Butce}  \tKaynak Kullanımı : {item.KaynakKullanim} \tSonuc: {item.Sonuc}");
                }

                //context.Model.Save(model, data.Schema, "model.zip");

                return Task.FromResult(newPre);
            }

            return null;
        }


        float GetSure(float sure)
        {
            if (sure >= 0 && sure < 2)
                return 5;
            else if (sure >= 2 && sure < 4)
                return 4;
            else if (sure >= 4 && sure < 6)
                return 3;
            else if (sure >= 6 && sure < 8)
                return 2;
            else
                return 1;
        }

        float GetButce(float butce)
        {
            if (butce >= 0 && butce < 20)
                return 1;
            else if (butce >= 20 && butce < 100)
                return 2;
            else if (butce >= 100 && butce < 300)
                return 3;
            else if (butce >= 300 && butce < 500)
                return 4;
            else
                return 5;
        }

        float GetTahminKar(float tahminKar)
        {
            if (tahminKar >= 0 && tahminKar < 10)
                return 1;
            else if (tahminKar >= 10 && tahminKar < 30)
                return 2;
            else if (tahminKar >= 30 && tahminKar < 50)
                return 3;
            else if (tahminKar >= 50 && tahminKar < 100)
                return 4;
            else
                return 5;
        }

        float GetIsgucu(float isGucu)
        {
            if (isGucu >= 0 && isGucu < 10)
                return 5;
            else if (isGucu >= 10 && isGucu < 50)
                return 4;
            else if (isGucu >= 50 && isGucu < 100)
                return 3;
            else if (isGucu >= 100 && isGucu < 300)
                return 2;
            else
                return 1;
        }

        float GetKaynak(float kaynak)
        {
            if (kaynak >= 0 && kaynak < 5)
                return 5;
            else if (kaynak >= 5 && kaynak < 20)
                return 4;
            else if (kaynak >= 20 && kaynak < 40)
                return 3;
            else if (kaynak >= 40 && kaynak < 60)
                return 2;
            else
                return 1;
        }

        float GetRisk(string risk)
        {
            if (risk.ToLower().Contains("çok düşük"))
                return 5;
            else if (risk.ToLower().Contains("çok yüksek"))
                return 4;
            else if (risk.ToLower().Contains("yüksek"))
                return 3;
            else if (risk.ToLower().Contains("orta"))
                return 2;
            else
                return 1;
        }

        void Present()
        {

            var dataFrame = DataFrame.LoadCsv("projeler-veri-csv.csv", separator: ';');

            var farkColumn = new PrimitiveDataFrameColumn<float>("Sure", dataFrame.Rows.Count);

            for (int i = 0; i < dataFrame.Rows.Count; i++)
            {
                DateTime baslangicTarihi = (DateTime)dataFrame[i, 2];
                DateTime bitisTarihi = (DateTime)dataFrame[i, 3];
                float fark = (float)Math.Round(bitisTarihi.Subtract(baslangicTarihi).TotalDays / 360);
                farkColumn[i] = fark;
            }

            dataFrame.Columns.Add(farkColumn);

            dataFrame.Columns.Remove("BaslangicTarihi");
            dataFrame.Columns.Remove("BitisTarihi");



            var risk1 = new PrimitiveDataFrameColumn<float>("Risk1", dataFrame.Rows.Count);
            var risk2 = new PrimitiveDataFrameColumn<float>("Risk2", dataFrame.Rows.Count);
            var risk3 = new PrimitiveDataFrameColumn<float>("Risk3", dataFrame.Rows.Count);
            var risk4 = new PrimitiveDataFrameColumn<float>("Risk4", dataFrame.Rows.Count);
            var risk5 = new PrimitiveDataFrameColumn<float>("Risk5", dataFrame.Rows.Count);

            for (int i = 0; i < dataFrame.Rows.Count; i++)
            {

                float sure, butce, tahminkar, isGucu, kaynak, _risk1, _risk2, _risk3, _risk4, _risk5;

                butce = GetButce((float)dataFrame[i, 2]);
                tahminkar = GetTahminKar((float)dataFrame[i, 3]);
                isGucu = GetIsgucu((float)dataFrame[i, 4]);
                kaynak = GetKaynak((float)dataFrame[i, 5]);
                _risk1 = GetRisk(dataFrame[i, 6].ToString());
                _risk2 = GetRisk(dataFrame[i, 7].ToString());
                _risk3 = GetRisk(dataFrame[i, 8].ToString());
                _risk4 = GetRisk(dataFrame[i, 9].ToString());
                _risk5 = GetRisk(dataFrame[i, 10].ToString());
                sure = GetSure((float)dataFrame[i, 12]);

                dataFrame[i, 2] = butce;
                dataFrame[i, 3] = tahminkar;
                dataFrame[i, 4] = isGucu;
                dataFrame[i, 5] = kaynak;
                dataFrame[i, 12] = sure;

                risk1[i] = _risk1;
                risk2[i] = _risk2;
                risk3[i] = _risk3;
                risk4[i] = _risk4;
                risk5[i] = _risk5;

                dataFrame[i, 11] = (float)(Math.Round(sure + butce + tahminkar + isGucu + kaynak + ((_risk1 + _risk2 + _risk3 + _risk4 + _risk5) / 5) / 10));
            }

            dataFrame.Columns.Remove("Risk1");
            dataFrame.Columns.Remove("Risk2");
            dataFrame.Columns.Remove("Risk3");
            dataFrame.Columns.Remove("Risk4");
            dataFrame.Columns.Remove("Risk5");
            dataFrame.Columns.Add(risk1);
            dataFrame.Columns.Add(risk2);
            dataFrame.Columns.Add(risk3);
            dataFrame.Columns.Add(risk4);
            dataFrame.Columns.Add(risk5);


            DataFrame.SaveCsv(dataFrame, "projeler-veri-csv.csv", separator: ';', header: true, encoding: Encoding.UTF8);
        }
    }

    public class ProjectGetData
    {
        public int Id { get; set; }
        public int Budget { get; set; }
        public int Revenue { get; set; }
        public int ManCount { get; set; }
        public int ResourcPercent { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public string? Risk1 { get; set; }
        public string? Risk2 { get; set; }
        public string? Risk3 { get; set; }
        public string? Risk4 { get; set; }
        public string? Risk5 { get; set; }
    }
    public class ProjectData
    {
        [LoadColumn(0)]
        public float Id { get; set; }
        [LoadColumn(1)] public string ProjeIsmi;
        [LoadColumn(2)] public string ProjeTipi;
        [LoadColumn(3)] public float Butce;
        [LoadColumn(4)] public float TahminiKar;
        [LoadColumn(5)] public float IsGucu;
        [LoadColumn(6)] public float KaynakKullanim;
      [LoadColumn(7)] public float Sonuc;
        [LoadColumn(8)] public float Sure;
        [LoadColumn(9)] public float Risk1;
        [LoadColumn(10)] public float Risk2;
        [LoadColumn(11)] public float Risk3;
        [LoadColumn(12)] public float Risk4;
        [LoadColumn(13)] public float Risk5;
    }

    // Tahmin sonuç sınıfı
    public class ProjectPrediction
    {
        [ColumnName("Id")]
        public float Id;

        [ColumnName("ProjeIsmi")]
        public string ProjeIsmi;

        [ColumnName("Butce")]
        public float Butce;

        [ColumnName("Sure")]
        public float Sure;

        [ColumnName("TahminiKar")]
        public float TahminiKar;

        [ColumnName("IsGucu")]
        public float IsGucu;

        [ColumnName("KaynakKullanim")]
        public float KaynakKullanim;

        [ColumnName("Score")]
        public float Sonuc;
    }
    public class ModelPredictionDto
    {
      public  List<ProjectGetData>? ItemList { get; set; }
        public string? strategyName { get; set; }
    }
}
