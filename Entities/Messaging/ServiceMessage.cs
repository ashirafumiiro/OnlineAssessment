using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Messaging
{
    public class ServiceMessage<T>
    {
        /// <summary>
        /// Gets or sets the error or warning code.
        /// </summary>
        public string ErrorOrWarningCode { get; set; }

        /// <summary>
        /// Gets or sets the error or warning narrative.
        /// </summary>
        public string ErrorOrWarningNarrative { get; set; }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        public T Data { get; set; }

        public void SetData(T data)
        {
            Data = data;
        }

        public bool HasErrors
        {
            get
            {
                return !string.IsNullOrEmpty(ErrorOrWarningCode) || !string.IsNullOrEmpty(ErrorOrWarningNarrative);
            }
        }
    }
}
