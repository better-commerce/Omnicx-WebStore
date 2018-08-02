using System;

namespace Omnicx.WebStore.Models.Infrastructure.Settings
{
    [Serializable]
    public class DomainSettings : ISettings
    {
        public string DomainName { get; set; }
        public string DomainCode { get; set; }
        /// <summary>
        /// Main domain URL associated with the domain. www.tfs.de
        /// </summary>
        public string DomainUrl { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public string LogoUrl { get; set; }

        //#region "Search Engine"
        //    public int SearchEngine { get; set; }
        //   // public string SearchEngineType { get; set; }
        //    public string SearchEngineIndexPath { get; set; }
        //    public string SearchEngineUserName { get; set; }
        //    public string SearchEnginePassword { get; set; }
        //    public string SearchDatabasePath { get; set; }
        //#endregion

        /// <summary>
        /// Default language culture for the domain
        /// </summary>
      
        
        public bool RedirectToNewDomain { get; set; }
     
    }
}
