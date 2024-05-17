using Google.Protobuf.WellKnownTypes;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Tls.Crypto;
using SUIVI.DataContexts;
using SUIVI.Models.AllModels;
using SUIVI.Models.AllModels.Suivimodel;
using SUIVI.Models.AllModels.SuiviModel;
using SUIVI.Repository.InterfaceRepository;
using System.Collections;
using System.Diagnostics.Metrics;
using System.Linq;

namespace SUIVI.Repository
{
    public class SuiviRepository : ISuiviRepository
    {
        public async Task<List<User_timer_pli>> TrackbetweenTwodate(DateTime Debut, DateTime Fin, string Name, string ConnectionString)
        {
            using (var context = RedergroupContext.RedergroupeContextConnectionString(ConnectionString))
            {
                return await (
                from timerpli in context.User_timer_pli
                where timerpli.End_date != null &&
                      timerpli.End_date.Value.Date >= Debut.Date &&
                      timerpli.End_date.Value.Date <= Fin.Date
                select new User_timer_pli
                {
                    Enseigne = timerpli.Enseigne,
                    Lotid = timerpli.Docid.Trim().Substring(0, timerpli.Docid.LastIndexOf('.')),
                    Docid = timerpli.Docid.Trim(),
                    Operateur = timerpli.Operateur,
                    Start_date = timerpli.Start_date,
                    End_date = timerpli.End_date,
                    Duration = timerpli.Duration,
                    Pli = timerpli.Pli,
                }
            ).ToListAsync();
            };
        }

        public async Task<IEnumerable<object>> CreateFormatResult(string Enseigne_name, IEnumerable<User_timer_pli> datas, string typesearch)
        {
            var result = Enumerable.Empty<Object>();
            if (typesearch == "enseigne") {
                result = datas
                    .GroupBy(x => new { x.Enseigne })
                    .Select(group => new
                    {
                        Enseigne = Enseigne_name,
                        CountLotglobal = group.GroupBy(item => item.Lotid).Count(),
                        Countplisglobal = group.GroupBy(item => new { item.Lotid, item.Pli }).Count(),
                        Countdocdistglobal = group.GroupBy(item => item.Docid).Count(),
                        CountvalidMultipleTimes = group.GroupBy(item => new { item.Lotid, item.Operateur }).Select(x => new { loid = x.Key.Lotid }).GroupBy(x => x.loid).Where(x => x.Count() > 1).Count(),
                        Startdate = group.Min(x => x.Start_date).ToString("dd/MM/yyyy HH:mm:ss"),
                        Enddate = group.Max(x => x.End_date).Value.ToString("dd/MM/yyyy HH:mm:ss"),
                        Durationglobal = group.Select(item => item.Duration)
                        .Aggregate(TimeSpan.Zero, (acc, next) => acc + next),
                        //ValidMultipleTimes = group
                        //    .GroupBy(x => new { x.Docid })
                        //    .Where(x => x.Count() > 1)
                        //    .Select(x => new {
                        //        Loid = x.Select(x => x.Lotid).FirstOrDefault(),
                        //        Docid = x.Select(x => x.Docid).FirstOrDefault(),
                        //        Count = x.Count(),
                        //        Details = x.Select(x => new {
                        //            Operateur = x.Operateur,
                        //            Startdate = x.Start_date.ToString("dd/MM/yyyy HH:mm:ss"),
                        //            Enddate = x.End_date.Value.ToString("dd/MM/yyyy HH:mm:ss"),
                        //            Duration = x.Duration
                        //        }).ToList(),
                        //    })
                        //    .ToList(),
                        ValidMultipleTimes = group
                            .GroupBy(item => new { item.Lotid, item.Operateur })
                            .Select(x => new {
                                Lotid = x.Select(x => x.Lotid).FirstOrDefault(),
                                Details = x.Select(x => new {
                                    Lotid = x.Lotid,
                                    Docid = x.Docid,
                                    Operateur = x.Operateur,
                                    Pli = "PLI-" + x.Pli,
                                    Startdate = x.Start_date.ToString("dd/MM/yyyy HH:mm:ss"),
                                    Enddate = x.End_date.Value.ToString("dd/MM/yyyy HH:mm:ss"),
                                    Duration = x.Duration
                                }).ToList()
                            })
                            .GroupBy(x => x.Lotid).Where(x => x.Count() > 1).ToList(),
                        Details = group
                            .GroupBy(item => new { item.Operateur })
                            .Select(detGroup => new
                            {
                                Operateur = detGroup.Select(x => x.Operateur).FirstOrDefault(),
                                CountLots = detGroup.GroupBy(item => item.Lotid).Count(),
                                Countplis = detGroup.GroupBy(item => new { item.Lotid, item.Pli }).Count(),
                                CountDocdist = detGroup.GroupBy(item => new { item.Docid }).Count(),
                                Startdate = detGroup.Min(x => x.Start_date).ToString("dd/MM/yyyy HH:mm:ss"),
                                Enddate = detGroup.Max(x => x.End_date).Value.ToString("dd/MM/yyyy HH:mm:ss"),
                                Duration = detGroup.Select(item => item.Duration)
                                .Aggregate(TimeSpan.Zero, (acc, next) => acc + next),
                                Detailspli = detGroup
                                    .GroupBy(item => new { item.Lotid, item.Pli })
                                    .Select(grouplot => new
                                    {
                                        Lotid = grouplot.Select(x => x.Lotid).FirstOrDefault(),
                                        Durationlot = detGroup
                                                              .GroupBy(item => new { item.Lotid })
                                                              .Where(x => x.Key.Lotid == grouplot.Select(x => x.Lotid).FirstOrDefault())
                                                              .Select(item => item.Select(item => item.Duration).Aggregate(TimeSpan.Zero, (acc, next) => acc + next))
                                                              .FirstOrDefault(),
                                        Pli = "PLI-" + grouplot.Select(x => x.Pli).FirstOrDefault(),
                                        CountDocdist = grouplot.GroupBy(item => new { item.Docid }).Count(),
                                        Startdate = grouplot.Min(x => x.Start_date).ToString("dd/MM/yyyy HH:mm:ss"),
                                        Enddate = grouplot.Max(x => x.End_date).Value.ToString("dd/MM/yyyy HH:mm:ss"),
                                        Duration = grouplot.Select(item => item.Duration)
                                                           .Aggregate(TimeSpan.Zero, (acc, next) => acc + next),
                                        //DetailPli = grouplot.GroupBy(item => item.Pli).Select(grouppli => new {
                                        //    Pli = "Pli" + grouppli.Select(x => x.Pli).FirstOrDefault(),

                                        //detaildocdist = grouppli.Select(item => new {
                                        //    Docid = item.Docid,
                                        //    Startdate = item.Start_date,
                                        //    Enddate = item.End_date,
                                        //    Duration = item.Duration,
                                        //}).ToList(),
                                        //})
                                        //.ToList(),
                                    }).OrderBy(x => x.Lotid)
                                    .ToList()
                            })
                            .ToList()
                    })
                    .ToList();
            }
            else
            {
                result = datas
                    .GroupBy(item => new { item.Operateur })
                    .Select(detGroup => new
                    {
                        Operateur = detGroup.Select(x => x.Operateur).FirstOrDefault(),
                        CountLots = detGroup.GroupBy(item => item.Lotid).Count(),
                        Countplis = detGroup.GroupBy(item => new { item.Lotid, item.Pli }).Count(),
                        CountDocdist = detGroup.GroupBy(item => new { item.Docid }).Count(),
                        Startdate = detGroup.Min(x => x.Start_date).ToString("dd/MM/yyyy HH:mm:ss"),
                        Enddate = detGroup.Max(x => x.End_date).Value.ToString("dd/MM/yyyy HH:mm:ss"),
                        Duration = detGroup.Select(item => item.Duration)
                        .Aggregate(TimeSpan.Zero, (acc, next) => acc + next),
                        Detailspli = detGroup
                            .GroupBy(item => new { item.Lotid, item.Pli })
                            .Select(grouplot => new
                            {
                                Lotid = grouplot.Select(x => x.Lotid).FirstOrDefault(),
                                Durationlot = detGroup
                                                        .GroupBy(item => new { item.Lotid })
                                                        .Where(x => x.Key.Lotid == grouplot.Select(x => x.Lotid).FirstOrDefault())
                                                        .Select(item => item.Select(item => item.Duration).Aggregate(TimeSpan.Zero, (acc, next) => acc + next))
                                                        .FirstOrDefault(),
                                Pli = "PLI-" + grouplot.Select(x => x.Pli).FirstOrDefault(),
                                CountDocdist = grouplot.GroupBy(item => new { item.Docid }).Count(),
                                Startdate = grouplot.Min(x => x.Start_date).ToString("dd/MM/yyyy HH:mm:ss"),
                                Enddate = grouplot.Max(x => x.End_date).Value.ToString("dd/MM/yyyy HH:mm:ss"),
                                Duration = grouplot.Select(item => item.Duration)
                                                    .Aggregate(TimeSpan.Zero, (acc, next) => acc + next),
                                //DetailPli = grouplot.GroupBy(item => item.Pli).Select(grouppli => new {
                                //    Pli = "Pli" + grouppli.Select(x => x.Pli).FirstOrDefault(),

                                //detaildocdist = grouppli.Select(item => new {
                                //    Docid = item.Docid,
                                //    Startdate = item.Start_date,
                                //    Enddate = item.End_date,
                                //    Duration = item.Duration,
                                //}).ToList(),
                                //})
                                //.ToList(),
                            }).OrderBy(x => x.Lotid)
                            .ToList()
                    })
                    .ToList();
            }
            return await Task.FromResult(result);
        }
    }
}
