using System;
using System.Collections.Concurrent;
using System.Linq;
using BannerAPI.Models;
using HtmlAgilityPack;

namespace BannerAPI.Repositories
{
    public class BannerRepository
    {
        public static class BannerDatabase
        {
            private static readonly ConcurrentDictionary<int, Banner> Banners = new ConcurrentDictionary<int, Banner>();

            public static bool Insert(Banner banner)
            {
                var now = DateTime.Now;
                return Banners.TryAdd(banner.Id, new Banner
                {
                    Id = banner.Id,
                    Created = now,
                    Modified = now,
                    Html = banner.Html
                });
            }

            public static Banner Get(int id)
            {
                Banner banner;
                return Banners.TryGetValue(id, out banner) ? banner : null;
            }

            public static bool Update(Banner banner)
            {
                var prev = Get(banner.Id);
                if (prev != null)
                {
                    Banners.AddOrUpdate(banner.Id, banner, (id, oldBanner) => new Banner()
                    {
                        Id = banner.Id,
                        Created = oldBanner.Created,
                        Html = banner.Html,
                        Modified = DateTime.Now
                    });
                }

                return false;
            }

            public static bool Delete(int id)
            {
                Banner banner;
                return Banners.TryRemove(id, out banner);
            }
        }

        public bool CreateBanner(Banner banner)
        {
            return BannerDatabase.Insert(banner);
        }

        public Banner ReadBanner(int id)
        {
            return BannerDatabase.Get(id);
        }

        public bool UpdateBanner(Banner banner)
        {
            return BannerDatabase.Update(banner);
        }

        public bool DeleteBanner(int id)
        {
            return BannerDatabase.Delete(id);
        }

        public string GetBannerHtml(int id)
        {
            var banner = ReadBanner(id);
            if (banner == null)
            {
                return null;
            }
            var doc = new HtmlDocument();
            doc.LoadHtml(banner.Html);
            return doc.ParseErrors.Any() ? null : banner.Html;
        }
    }
}