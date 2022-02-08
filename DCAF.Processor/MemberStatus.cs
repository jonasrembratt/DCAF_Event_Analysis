using System;
using DCAF.Inspection._lib;

namespace DCAF.Inspection
{
    public enum MemberStatus
    {
        Pending,
        
        Active,
        
        LOA,
        
        AWOL,
        
        ExtenuatingCircumstances,
        
        FormalDischargeLWR,
        
        Banned
    }

    public static class MemberStatusHelper
    {
        public static bool TryParseMemberStatus(this string s, out MemberStatus? status)
        {
            var ident = s.ToIdentifier(StringHelper.IdentifierCasing.Pascal);
            if (Enum.TryParse(typeof(MemberStatus), s, true, out var e))
            {
                status = (MemberStatus) e!;
                return true;
            }

            status = null;
            return false;
        }
    }
    
}