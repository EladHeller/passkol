using Repository.UnitOfWork;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Infrastructure.Email;
using Infrastructure.Configuration;
using Services.PDFService;

namespace Services
{
    public class ServiceLocator 
    {
        // map that contains pairs of interfaces and
        // references to concrete implementations
        private static IDictionary<object, object> services;

        static ServiceLocator()
        {
            services = new Dictionary<object, object>();
            UnitOfWork uow = new UnitOfWork();
            // fill the map
            services.Add(typeof(TagService), new TagService(new TagsRepository(uow), uow));
            services.Add(typeof(PDFServiceSoapClient), new PDFServiceSoapClient());
            services.Add(typeof(MailUnsubscribeService), new MailUnsubscribeService(new MailUnsubscribeRepository(uow), uow));
            services.Add(typeof(PaskolEmailService), new PaskolEmailService(new GoogleMail(), new MailUnsubscribeRepository(uow)));
            services.Add(typeof(FileSystemService), new FileSystemService(new FileSystemRepository(WebConf.FSBaseRoute)));
           
            services.Add(typeof(ReportService) ,new ReportService(new PurchaseRepository(uow), new LogDataRepository(uow),uow));
            services.Add(typeof(PurchaseService), new PurchaseService(
                services[typeof(PaskolEmailService)] as PaskolEmailService,
                new PurchaseRepository(uow),
                new UsersRepository(uow),
                new MusicsRepository(uow),
                new PermissionRepository(uow),
                uow));
            services.Add(typeof(PermissionService), new PermissionService(new PermissionRepository(uow), uow));
            services.Add(typeof(UsersService), new UsersService(
                new UsersRepository(uow),
                services[typeof(PaskolEmailService)] as PaskolEmailService, 
                uow));
            services.Add(typeof(UserActionService), new UserActionService(new UserActiveLogRepository(uow), uow));
            services.Add(typeof(PermissionCategoryService), new PermissionCategoryService(new PermissionCategoryRepository(uow), uow, services[typeof(PermissionService)] as PermissionService, new MusicsRepository(uow)));
            services.Add(typeof(PermissionPropertyService), new PermissionPropertyService(new PermissionPropertyRepository(uow), uow));
            services.Add(typeof(AuctionService), new AuctionService(
                services[typeof(PermissionCategoryService)] as PermissionCategoryService,
                services[typeof(PaskolEmailService)] as PaskolEmailService,
                new AuctionRepository(uow), 
                new UsersRepository(uow), 
                uow));
            services.Add(typeof(MusicService), new MusicService(new MusicsRepository(uow), uow,
               services[typeof(FileSystemService)] as FileSystemService,
               services[typeof(TagService)] as TagService,
               new UsersRepository(uow),
               services[typeof(AuctionService)] as AuctionService));
            services.Add(typeof(ConfirmationService), new ConfirmationService(new ConfirmationRepository(uow),
               new PurchaseRepository(uow),
               new MusicsRepository(uow), uow));
            services.Add(typeof(NCRegisterService), new NCRegisterService(new UsersRepository(uow), new NCRegisterRepository(uow), uow));
            services.Add(typeof(NCDataUploadService), new NCDataUploadService(new MusicsRepository(uow),new NCDataUploadRepository(uow), uow));
            services.Add(typeof(NCPurchaseService), new NCPurchaseService(new PurchaseRepository(uow),new NCPurchaseRepository(uow), uow));
            services.Add(typeof(DashboardService),
                         new DashboardService(uow,
                                             new NCRegisterRepository(uow),
                                             new NCDataUploadRepository(uow),
                                             new NCPurchaseRepository(uow),
                                             new AuctionRepository(uow),
                                             new PurchaseRepository(uow),
                                             new ConfirmationRepository(uow)));
            services.Add(typeof(SearchLogService), new SearchLogService(new SearchLogRepository(uow), uow));
        }

        public static T GetService<T>()
        {
            try
            {
                return (T)services[typeof(T)];
            }
            catch (KeyNotFoundException)
            {
                throw new ApplicationException("The requested service is not registered");
            }
        }
    }
}
