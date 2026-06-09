using SD.Core.Entities;
using SD.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace SD.Core.EnumDescriptors
{
    public sealed class RatingsDescriptor : EnumDescriptorBase<Ratings>
    {
        private RatingsDescriptor(Ratings rating, string code, string resourceKey)
          : base(rating, code, resourceKey) { }


        /* Static instances */
        public static readonly RatingsDescriptor Unrated = new(Ratings.Unrated, "0", nameof(BasicRes.Ratings_0));
        public static readonly RatingsDescriptor Bad = new(Ratings.Bad, "10", nameof(BasicRes.Ratings_10));
        public static readonly RatingsDescriptor Medium = new(Ratings.Average, "20", nameof(BasicRes.Ratings_20));
        public static readonly RatingsDescriptor Great = new(Ratings.Good, "30", nameof(BasicRes.Ratings_30));
        public static readonly RatingsDescriptor Excellent = new(Ratings.Excellent, "40", nameof(BasicRes.Ratings_40));

        // All iteration values
        public static IEnumerable<RatingsDescriptor> All => [Unrated, Bad, Medium, Great, Excellent];


        public static bool TryFromCode(string code, out RatingsDescriptor? ratingsDescriptor)
        {
            ratingsDescriptor = All.FirstOrDefault(x => x.Code == code);
            return ratingsDescriptor is not null;
        }

        public static Guid CodeFromEnum(Ratings rating)
        {
            string code = All.FirstOrDefault(w => w.Enum == rating)?.Code ?? throw new ArgumentException($"Unknown breed type enum: {rating.ToString()}", nameof(rating));
            return new Guid(code);
        }

        public static RatingsDescriptor FromEnum(Ratings rating)
        {
            return All.FirstOrDefault(w => w.Enum == rating) ?? throw new ArgumentException($"Unknown breed type enum: {rating.ToString()}", nameof(rating));
        }

        public static IEnumerable<string> Names => All.Select(x => x.ToString());

    }
}
