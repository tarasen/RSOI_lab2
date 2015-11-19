using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace PictureAuction.Api.ServiceModel
{
    [DataContract]
    public abstract class PageResult
    {
        private long? _count;

        protected PageResult(Uri nextPageLink, long? count)
        {
            NextPageLink = nextPageLink;
            Count = count;
        }

        [DataMember]
        public Uri NextPageLink { get; private set; }

        [DataMember]
        public long? Count
        {
            get { return _count; }
            private set
            {
                if (value.HasValue && value.Value < 0L)
                    throw new ArgumentOutOfRangeException(nameof(value), value.Value, "");
                _count = value;
            }
        }
    }

    [DataContract]
    public class PageResult<T> : PageResult
    {
        public PageResult(IEnumerable<T> items, Uri nextPageLink, long? count)
            : base(nextPageLink, count)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));
            Items = items;
        }

        [DataMember]
        public IEnumerable<T> Items { get; private set; }
    }
}