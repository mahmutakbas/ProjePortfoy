using AutoMapper;
using Entities.Concrete;
using Entities.DTOs;

namespace PortfoyAPI.Models
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Departman, DepartmanDTO>()
                .ForMember(d => d.Id, opt => opt.MapFrom(x => x.Id))
                .ForMember(d => d.Name, opt => opt.MapFrom(x => x.DepartmanAdi));

            CreateMap<DepartmanDTO, Departman>()
                .ForMember(d => d.Id, opt => opt.MapFrom(x => x.Id))
                .ForMember(d => d.DepartmanAdi, opt => opt.MapFrom(x => x.Name));

            CreateMap<Proje, ProjectDto>()
                .ForMember(d => d.Id, opt => opt.MapFrom(x => x.Id))
                .ForMember(d => d.Name, opt => opt.MapFrom(x => x.ProjeAdi))
                .ForMember(d => d.Type, opt => opt.MapFrom(x => x.ProjeKategoriId))
                .ForMember(d => d.StartDate, opt => opt.MapFrom(x => x.BaslangicTarihi))
                .ForMember(d => d.FinishDate, opt => opt.MapFrom(x => x.BitisTarihi))
                .ForMember(d => d.Status, opt => opt.MapFrom(x => x.ProjeDurum))
                .ForMember(d => d.Budget, opt => opt.MapFrom(x => x.ProjeButcesi))
                .ForMember(d => d.Revenue, opt => opt.MapFrom(x => x.ProjeGeliri))
                .ForMember(d => d.Description, opt => opt.MapFrom(x => x.ProjeAciklama))
                .ForMember(d => d.Customer, opt => opt.MapFrom(x => x.ProjeMusteri))
                .ForMember(d => d.DepartmentId, opt => opt.MapFrom(x => x.DepartmanId));

            CreateMap<ProjectDto, Proje>()
                .ForMember(d => d.Id, opt => opt.MapFrom(x => x.Id))
                .ForMember(d => d.ProjeAdi, opt => opt.MapFrom(x => x.Name))
                .ForMember(d => d.ProjeKategoriId, opt => opt.MapFrom(x => x.Type))
                .ForMember(d => d.BaslangicTarihi, opt => opt.MapFrom(x => x.StartDate))
                .ForMember(d => d.BitisTarihi, opt => opt.MapFrom(x => x.FinishDate))
                .ForMember(d => d.ProjeDurum, opt => opt.MapFrom(x => x.Status))
                .ForMember(d => d.ProjeButcesi, opt => opt.MapFrom(x => x.Budget))
                .ForMember(d => d.ProjeGeliri, opt => opt.MapFrom(x => x.Revenue))
                .ForMember(d => d.ProjeAciklama, opt => opt.MapFrom(x => x.Description))
                .ForMember(d => d.ProjeMusteri, opt => opt.MapFrom(x => x.Customer))
                .ForMember(d => d.DepartmanId, opt => opt.MapFrom(x => x.DepartmentId));

            CreateMap<KPI, KPIDto>()
                .ForMember(d => d.Name, opt => opt.MapFrom(x => x.KPIAdi))
                .ForMember(d => d.Goal, opt => opt.MapFrom(x => x.Puan));

            CreateMap<KPIDto, KPI>()
                .ForMember(d => d.Id, opt => opt.MapFrom(x => x.Id))
                .ForMember(d => d.KPIAdi, opt => opt.MapFrom(x => x.Name))
                .ForMember(d => d.Puan, opt => opt.MapFrom(x => x.Goal));

            CreateMap<ProjeDetay, ProjeDetayDto>()
                    .ForMember(d => d.Id, opt => opt.MapFrom(x => x.Id))
                    .ForMember(d => d.ProjectId, opt => opt.MapFrom(x => x.ProjeId))
                    .ForMember(d => d.Name, opt => opt.MapFrom(x => x.Aciklama))
                    .ForMember(d => d.StartDate, opt => opt.MapFrom(x => x.BaslangicTarihi))
                    .ForMember(d => d.FinishDate, opt => opt.MapFrom(x => x.BitisTarihi))
                    .ForMember(d => d.Status, opt => opt.MapFrom(x => x.Durum));

            CreateMap<ProjeDetayDto, ProjeDetay>()
                .ForMember(d => d.Id, opt => opt.MapFrom(x => x.Id))
                .ForMember(d => d.ProjeId, opt => opt.MapFrom(x => x.ProjectId))
                .ForMember(d => d.Aciklama, opt => opt.MapFrom(x => x.Name))
                .ForMember(d => d.BaslangicTarihi, opt => opt.MapFrom(x => x.StartDate))
                .ForMember(d => d.BitisTarihi, opt => opt.MapFrom(x => x.FinishDate))
                .ForMember(d => d.Durum, opt => opt.MapFrom(x => x.Status));

            CreateMap<Risk, RiskDto>()
                .ForMember(d => d.Id, opt => opt.MapFrom(x => x.Id))
                .ForMember(d => d.Name, opt => opt.MapFrom(x => x.RiskTanimi))
                .ForMember(d => d.Status, opt => opt.MapFrom(x => x.RiskDurumu));

            CreateMap<RiskDto, Risk>()
                .ForMember(d => d.Id, opt => opt.MapFrom(x => x.Id))
                .ForMember(d => d.RiskTanimi, opt => opt.MapFrom(x => x.Name))
                .ForMember(d => d.RiskDurumu, opt => opt.MapFrom(x => x.Status));


        }
    }
}
