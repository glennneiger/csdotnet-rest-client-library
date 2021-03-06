﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using Newtonsoft.Json.Linq;
using VVRestApi.Common.Messaging;
using VVRestApi.Vault.Groups;

namespace VVRestApi.Vault.Users
{
    using VVRestApi.Common;

    /// <summary>
    /// 
    /// </summary>
    public class UsersManager : VVRestApi.Common.BaseApi
    {
        internal UsersManager(VaultApi api)
        {
            base.Populate(api.ClientSecrets, api.ApiTokens);
        }

        /// <summary>
        /// creates a new group in the site
        /// </summary>
        /// <returns></returns>
        public User CreateUser(Guid siteId, string userId, string firstName, string middleInitial, string lastName, string emailAddress, string password, bool passwordNeverExpires, DateTime passwordExpiresDate)
        {
            dynamic postData = new ExpandoObject();
            postData.siteId = siteId;
            postData.userId = userId;
            postData.firstName = firstName;
            postData.middleInitial = middleInitial;
            postData.lastName = lastName;
            postData.emailAddress = emailAddress;
            postData.password = password;
            postData.passwordNeverExpires = passwordNeverExpires;
            postData.passwordExpires = passwordExpiresDate.ToString("s");

            return HttpHelper.Post<User>(VVRestApi.GlobalConfiguration.Routes.Users, "", GetUrlParts(), this.ClientSecrets, this.ApiTokens, postData);
        }

        /// <summary>
        /// creates a new group in the site
        /// </summary>
        /// <returns></returns>
        public User CreateUser(Guid siteId, string userId, string firstName, string middleInitial, string lastName, string emailAddress, string password)
        {
            dynamic postData = new ExpandoObject();
            postData.siteId = siteId;
            postData.userId = userId;
            postData.firstName = firstName;
            postData.middleInitial = middleInitial;
            postData.lastName = lastName;
            postData.emailAddress = emailAddress;
            postData.password = password;

            return HttpHelper.Post<User>(VVRestApi.GlobalConfiguration.Routes.Users, "", GetUrlParts(), this.ClientSecrets, this.ApiTokens, postData);
        }

        /// <summary>
        /// Gets a page of users with no filter
        /// </summary>
        /// <param name="options"> </param>
        /// <returns></returns>
        public Page<User> GetUsers(RequestOptions options = null)
        {
            var results = HttpHelper.GetPagedResult<User>(GlobalConfiguration.Routes.Users, "", options, GetUrlParts(), this.ClientSecrets, this.ApiTokens);
            
            return results;
        }

        public Page<User> GetDisabledUsers(RequestOptions options = null)
        {
            var queryString = "enabled=false";
            var results = HttpHelper.GetPagedResult<User>(GlobalConfiguration.Routes.Users, queryString, options, GetUrlParts(), this.ClientSecrets, this.ApiTokens);

            return results;
        }

        public Page<User> GetAllEnabledAndDisabledUsers(RequestOptions options = null)
        {
            var queryString = "enabled=all";
            var results = HttpHelper.GetPagedResult<User>(GlobalConfiguration.Routes.Users, queryString, options, GetUrlParts(), this.ClientSecrets, this.ApiTokens);

            return results;
        }

        public Page<User> GetAccountOwnersList(RequestOptions options = null)
        {
            var queryString = "isVaultAccess=true";
            var results = HttpHelper.GetPagedResult<User>(GlobalConfiguration.Routes.Users, queryString, options, GetUrlParts(), this.ClientSecrets, this.ApiTokens);

            return results;
        }




        /// <summary>
        /// Gets a user by their username
        /// </summary>
        /// <param name="username"></param>
        /// <param name="options"> </param>
        /// <returns></returns>
        public User GetUser(string username, RequestOptions options = null)
        {
            return HttpHelper.Get<User>(GlobalConfiguration.Routes.UsersDefaultCustomer, string.Format("q=[userid] eq '{0}'", username), options, GetUrlParts(),this.ClientSecrets, this.ApiTokens);
        }

        /// <summary>
        /// returns the users default CustomerDatabase info object
        /// </summary>
        /// <returns></returns>
        public dynamic GetUserDefaultCustomerAndDatabaseInfo()
        {
            return HttpHelper.Get<DefaultCustomerInfo>(VVRestApi.GlobalConfiguration.Routes.UsersDefaultCustomer, "", null, GetUrlParts(), this.ClientSecrets, this.ApiTokens, false, null);
        }

        /// <summary>
        /// sends invitations to co-workers to become users
        /// </summary>
        /// <param name="userList"></param>
        /// <param name="baseUrl"></param>
        /// <param name="message"></param>
        /// <returns></returns>

        public User AcceptUserInvite(string link, string email, string firstName, string lastName, string password)
        {
            dynamic postData = new ExpandoObject();
            postData.email = email;
            postData.password = password;
            postData.firstName = firstName;
            postData.lastName = lastName;

            return HttpHelper.PostPublicNoCustomerAliases<User>(VVRestApi.GlobalConfiguration.Routes.InviteId, "", GetUrlParts(), this.ApiTokens, postData, link);

        }

        public User SetUserAsAccountOwner(Guid usId)
        {
            dynamic postData = new ExpandoObject();
            postData.isVaultAccess = true;

            return HttpHelper.Put<User>(VVRestApi.GlobalConfiguration.Routes.UsersId, "", GetUrlParts(), ClientSecrets, this.ApiTokens, postData, usId);
        }

        public User RemoveUserFromAccountOwner(Guid usId)
        {
            dynamic postData = new ExpandoObject();
            postData.isVaultAccess = false;

            return HttpHelper.Put<User>(VVRestApi.GlobalConfiguration.Routes.UsersId, "", GetUrlParts(), ClientSecrets, this.ApiTokens, postData, usId);
        }

        public User ChangeUserPassword(Guid usId, string oldPassword, string newPassword)
        {
            dynamic postData = new ExpandoObject();
            postData.oldPassword = oldPassword;
            postData.newPassword = newPassword;

            return HttpHelper.Put<User>(VVRestApi.GlobalConfiguration.Routes.UsersIdPassword, "", GetUrlParts(), ClientSecrets, this.ApiTokens, postData, usId);
        }

        public User ChangeUserFirstNameAndLastName(Guid usId, string firstName, string lastName)
        {
            dynamic postData = new ExpandoObject();
            postData.firstName = firstName;
            postData.lastName = lastName;

            return HttpHelper.Put<User>(VVRestApi.GlobalConfiguration.Routes.UsersId, "", GetUrlParts(), ClientSecrets, this.ApiTokens, postData, usId);
        }

        public User DisableUser(Guid usId)
        {
            dynamic postData = new ExpandoObject();
            postData.enabled = false;

            return HttpHelper.Put<User>(VVRestApi.GlobalConfiguration.Routes.UsersId, "", GetUrlParts(), ClientSecrets, this.ApiTokens, postData, usId);
        }

        public User EnableUser(Guid usId)
        {
            dynamic postData = new ExpandoObject();
            postData.enabled = true;

            return HttpHelper.Put<User>(VVRestApi.GlobalConfiguration.Routes.UsersId, "", GetUrlParts(), ClientSecrets, this.ApiTokens, postData, usId);
        }

        public User ResetPassword(Guid usId, bool forceLinkUsage)
        {
            dynamic postData = new ExpandoObject();
            postData.baseUrl = BaseApi.BaseUrl;
            postData.resetPassword = true;
            postData.forceEmailLink = forceLinkUsage;

            return HttpHelper.Put<User>(VVRestApi.GlobalConfiguration.Routes.UsersIdPassword, "", GetUrlParts(), ClientSecrets, this.ApiTokens, postData, usId);
        }

        public void CheckValidPasswordResetToken(string passwordResetToken)
        {
            HttpHelper.GetPublicNoCustomerAliases(GlobalConfiguration.Routes.UsersResetId, "", GetUrlParts(), passwordResetToken);
        }

        public void SetUserPasswordWithPasswordResetToken(string newPassword, string passwordResetToken)
        {
            dynamic postData = new ExpandoObject();
            postData.newPassword = newPassword;

            HttpHelper.PutPublicNoCustomerAliases(GlobalConfiguration.Routes.UsersResetId, "", GetUrlParts(), this.ApiTokens, postData, passwordResetToken);
        }


    }


}