using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Osrm.Client.Models.Requests
{
    public class NearestRequest : BaseRequest
    {
        private const uint DefaultNumber = 1;

        public NearestRequest()
        {
            Number = DefaultNumber;
        }

        /// <summary>
        /// Number of nearest segments that should be returned.
        /// integer >= 1 (default 1)
        /// </summary>
        public uint Number { get; set; }

        public override List<Tuple<string, string>> UrlParams
        {
            get
            {
                var urlParams = new List<Tuple<string, string>>(BaseUrlParams);

                urlParams
                    .AddStringParameter("number", Number.ToString(), () => Number != DefaultNumber);

                return urlParams;
            }
        }

        internal override void Validate()
        {
            ValidateCoordinateCount(1, 1);
            ValidateOptionalParameterLengths();
        }
    }
}