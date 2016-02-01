using System;

namespace crds_angular.Util.Interfaces
{
    public interface IDateTime
    {
        DateTime Now { get; }
        DateTime Today { get; }
    }
}