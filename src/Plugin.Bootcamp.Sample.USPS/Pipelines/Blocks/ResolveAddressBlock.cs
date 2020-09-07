using Plugin.Bootcamp.Sample.USPS.DTO;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;
using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Plugin.Bootcamp.Sample.USPS.Pipelines.Blocks
{
    [PipelineDisplayName("ResolveAddressBlock")]
    public class ResolveAddressBlock : PipelineBlock<Party, Party, CommercePipelineExecutionContext>
    {
        public override Task<Party> Run(Party arg, CommercePipelineExecutionContext context)
        {
            Contract.Requires(arg != null);
            Contract.Requires(context != null);

            var policy = context.CommerceContext.GetPolicy<Policies.USPSPolicy>();
            var newParty = arg;

            /* STUDENT: Complete these method to match the requirements. You will need to:
             * 1. Get the address from the input argument
             * 2. Create an instance of USPSAddressValidationRequest and set its properties
             * 3. Call the USPSValidate function
             * 4. Map the properties of its return object to the newParty object
             */
            

            return Task.FromResult(newParty);
        }

        private static USPSAddressValidationResponse USPSValidate(USPSAddressValidationRequest request, Policies.USPSPolicy policy)
        {
            var s = System.Xml.Serialization.XmlSerializer.FromTypes(new Type[] { typeof(USPSAddressValidationRequest), typeof(USPSAddressValidationResponse) });
            var sb = new System.Text.StringBuilder();

            using (var w = new System.IO.StringWriter(sb))
            {
                s[0].Serialize(w, request);
                w.Flush();
            }

            if (!policy.DebugMode)
            {
                //This Code is the 'real' code - commment out the live service call for the bootcamp
                String rawResponse = string.Empty;
                var url = new Uri($"{policy.Url}{sb.ToString()}");
                using (var client = new HttpClient())
                {
                    rawResponse = client.GetStringAsync(url).Result;
                }

                if (rawResponse.Contains("<Error>"))
                {
                    var rdr = XElement.Parse(rawResponse);
                    var errMsg = (from item in rdr.Descendants("Description")
                                  select item.Value).Single();

                    return null;
                }
                else
                {
                    USPSAddressValidationResponse resp = null;
                    using (var r = XmlReader.Create(new StringReader(rawResponse)))
                    {
                        resp = s[1].Deserialize(r) as USPSAddressValidationResponse;
                    }

                    return resp;
                }
            }
            else
            {
                USPSAddressValidationResponse resp = new USPSAddressValidationResponse()
                {
                    Address = new USPSAddress()
                    {
                        Id = "STUB",
                        Address1 = "123 Plugin Lane",
                        Address2 = String.Empty,
                        City = "Bootcamp",
                        State = "OH",
                        Zip5 = "43015",
                        Zip4 = "4333"
                    }
                };

                return resp;
            }
        }
    }
}
