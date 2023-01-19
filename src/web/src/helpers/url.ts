import { Const } from "./const";
import { UrlLinkage } from "./base";

const URL = {
    User: {
        GetItem: UrlLinkage(Const.APIURI, "User"),
        GetPagedList: UrlLinkage(Const.APIURI, "User", "Filter"),
        Login: UrlLinkage(Const.APIURI, "User/Login"),
        GetData: UrlLinkage(Const.APIURI, "User/Data"),
        Delete: UrlLinkage(Const.APIURI, "User"),
        Update: UrlLinkage(Const.APIURI, "User"),
        Insert: UrlLinkage(Const.APIURI, "User"),
        Role: UrlLinkage(Const.APIURI, "User", "Role"),
        Active: UrlLinkage(Const.APIURI, "User", "Active"),
        Deactive: UrlLinkage(Const.APIURI, "User", "Deactive"),
        Unlock: UrlLinkage(Const.APIURI, "User", "Unlock"),
        ResetPassword: UrlLinkage(Const.APIURI, "User", "ResetPassword/"),
        ChangePassword: UrlLinkage(Const.APIURI, "User", "ChangePassword"),
        GetSelectList: UrlLinkage(Const.APIURI, "User", "Select"),
        GetCaptcha: UrlLinkage(Const.APIURI, "User/GetCaptcha"),
    },
    Role: {
        GetItem: UrlLinkage(Const.APIURI, "Role"),
        GetPagedList: UrlLinkage(Const.APIURI, "Role", "Filter"),
        GetSelectList: UrlLinkage(Const.APIURI, "Role"),
        Delete: UrlLinkage(Const.APIURI, "Role"),
        Update: UrlLinkage(Const.APIURI, "Role"),
        Insert: UrlLinkage(Const.APIURI, "Role"),
        GetPermission: UrlLinkage(Const.APIURI, "Role", "Permission"),
    },
  
    Permission: {
        GetList: UrlLinkage(Const.APIURI, "Permission"),
        All: UrlLinkage(Const.APIURI, "Permission", "All"),
    },
  
 
    SLADBForm: {
        GetSelectList: UrlLinkage(Const.APIURI, "SLADBForm"),
        GetPagedList: UrlLinkage(Const.APIURI, "SLADBForm", "Filter"),
        Delete: UrlLinkage(Const.APIURI, "SLADBForm"),
        GetSourceColumn: UrlLinkage(Const.APIURI, "SLADBForm", "GetSourceColumn"),
        GetSourceColumnItem: UrlLinkage(Const.APIURI, "SLADBForm", "GetSourceColumnItem"),
        GetCategorySelectedList: UrlLinkage(Const.APIURI, "SLADBForm", "GetCategorySelectedList"),
        Insert: UrlLinkage(Const.APIURI, "SLADBForm"),
        Update: UrlLinkage(Const.APIURI, "SLADBForm"),

    },
};

export default URL;
