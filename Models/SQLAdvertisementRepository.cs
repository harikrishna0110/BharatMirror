using BharatMirror.Data;

namespace BharatMirror.Models
{

    public class SQLAdvertisementRepository : IAdvertisementRepository
    {
        private readonly AppDbContext context;
        public SQLAdvertisementRepository(AppDbContext context)
        {
            this.context = context;
        }
        Advertisement IAdvertisementRepository.Add(Advertisement Advertisement)
        {
            context.Advertisement.Add(Advertisement);
            context.SaveChanges();
            return Advertisement;
        }

        Advertisement IAdvertisementRepository.Delete(int Id)
        {
            Advertisement ad = context.Advertisement.Find(Id);
            if (ad != null)
            {
                context.Advertisement.Remove(ad);
                context.SaveChanges();
            }
            return ad;
        }

        Advertisement IAdvertisementRepository.GetAdvertisement(int Id)
        {
            return context.Advertisement.FirstOrDefault(m => m.Id == Id);
        }

        IEnumerable<Advertisement> IAdvertisementRepository.GetAdvertisements(string category)
        {
            DateTime ctime = DateTime.Now;
            IEnumerable<Advertisement> advertisements = context.Advertisement.Where(m => m.category == category && m.status == "Accepted" && ctime.CompareTo(m.start_time) > 0 && ctime.CompareTo(m.end_time) < 0);
            int size = advertisements.Count();
            for (int i = 0; i < size; i++)
            {
                advertisements.ElementAt(i).views += 1;
            }
            context.UpdateRange(advertisements);
            context.SaveChanges();
            return advertisements;
        }

        IEnumerable<Advertisement> IAdvertisementRepository.GetAdvertisements_admin(string status)
        {
            return context.Advertisement.Where(m => m.status == status && m.payment == "Done");
        }

        IEnumerable<Advertisement> IAdvertisementRepository.GetAdvertisements_user(int userId)
        {
            return context.Advertisement.Where(m => m.user_id == userId);
        }

        Advertisement IAdvertisementRepository.Update(Advertisement AdvertisementChanges)
        {
            if(AdvertisementChanges.reason == null)
            {
                AdvertisementChanges.reason = "None";
            }
            var Advertisement = context.Advertisement.Attach(AdvertisementChanges);
            Advertisement.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return AdvertisementChanges;
        }


        void IAdvertisementRepository.AddCtr(int Id)
        {
            Advertisement advertisement = context.Advertisement.FirstOrDefault(m => m.Id == Id);
            advertisement.ctr += 1;
            var ad = context.Advertisement.Attach(advertisement);
            ad.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
        }
    }
}




