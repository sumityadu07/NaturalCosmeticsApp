
using Swibbl.Services;
using System;

namespace Swibbl.Models.Marketing
{
    public class Campaign : IIdentifiable
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public string Img { get; set; }
    }
}
