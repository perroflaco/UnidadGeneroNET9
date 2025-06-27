using System;
using Microsoft.Extensions.Configuration;

namespace SEV.Library
{
    public class PartnerValidator
    {
        private IConfiguration Configuration { get; set; }
        private PartnerData CurrentPartner { get; set; }
        public PartnerValidator()
        {
            Configuration = SEVConfigAssistant.Configuration;
            CurrentPartner = new PartnerData();
        }

        public PartnerValidator FindPartner(String PartnerLabel)
        {
            CurrentPartner = new PartnerData()
            {
                Id = Configuration["Partners:" + PartnerLabel + ":Id"],
                Key = Configuration["Partners:" + PartnerLabel + ":Key"]
            };
            return this;
        }

        public Boolean IsValid(String Id, String Key)
        {
            Boolean Result = (
                CurrentPartner.Id == Id
                &&
                CurrentPartner.Key == Key
            );
            return Result;
        }
    }

    public class PartnerData{
        public String Id { get; set; }
        public String Key { get; set; }
    }
}
