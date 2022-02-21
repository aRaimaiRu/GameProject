using System.Collections.Generic;
using UnityEngine;

namespace RoleList
{


    public static class RoleListClass
    {
        public enum RoleList
        {
            Process,
            Scanner,
            Deleter,
            Worm,
            Spyware,
            Imposter
        }

        public static Dictionary<RoleList, string> allroledict = new Dictionary<RoleList, string>(){
        {RoleList.Process,"process"},
        {RoleList.Scanner,"Scanner"},
        {RoleList.Deleter,"Deleter"},
        {RoleList.Worm,"Worm"},
        {RoleList.Spyware,"Spyware"}
    };
        public static List<RoleList> VirusRoleList = new List<RoleList>() {
        RoleList.Worm,
        RoleList.Spyware
        };
        public static List<RoleList> AntiVirusRoleList = new List<RoleList>() {
        RoleList.Process,
        RoleList.Scanner,
        RoleList.Deleter,
        };

    }


}
