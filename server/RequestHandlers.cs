#region

using db;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Web;

#endregion

namespace server
{
    public abstract class RequestHandler
    {
        protected NameValueCollection Query { get; private set; }
        protected HttpListenerContext Context { get; private set; }

        public void HandleRequest(HttpListenerContext context)
        {
            this.Context = context;
            if (ParseQueryString())
            {
                Query = new NameValueCollection();
                using (StreamReader rdr = new StreamReader(context.Request.InputStream))
                    Query = HttpUtility.ParseQueryString(rdr.ReadToEnd());

                if (Query.AllKeys.Length == 0)
                {
                    string currurl = context.Request.RawUrl;
                    int iqs = currurl.IndexOf('?');
                    if (iqs >= 0)
                        Query = HttpUtility.ParseQueryString((iqs < currurl.Length - 1) ? currurl.Substring(iqs + 1) : string.Empty);
                }
            }

            HandleRequest();
        }

        public bool CheckAccount(Account acc, Database db, bool checkAccInUse=true)
        {
            if (acc == null && !String.IsNullOrWhiteSpace(Query["password"]))
            {
                WriteErrorLine("Account credentials not valid");
                return false;
            }
            else if (acc == null && String.IsNullOrWhiteSpace(Query["password"]))
                return true;

            if (acc.Banned)
            {
                using (StreamWriter wtr = new StreamWriter(Context.Response.OutputStream))
                    wtr.WriteLine("<Error>Account under maintenance</Error>");
                Context.Response.Close();
                return false;
            }
            if (checkAccInUse)
            {
                int? timeout = 0;
                if (db.CheckAccountInUse(acc, ref timeout))
                {
                    if (timeout != null)
                        using (StreamWriter wtr = new StreamWriter(Context.Response.OutputStream))
                            wtr.WriteLine("<Error>Account in use. (" + timeout + " seconds until timeout.)</Error>");
                    else
                        using (StreamWriter wtr = new StreamWriter(Context.Response.OutputStream))
                            wtr.WriteLine("<Error>Account in use.</Error>");

                    Context.Response.Close();
                    return false;
                }
            }
            return true;
        }

        public void WriteLine(string value, params object[] args)
        {
            using (StreamWriter wtr = new StreamWriter(Context.Response.OutputStream))
                if (args == null || args.Length == 0) wtr.Write(value);
                else wtr.Write(value, args);
        }

        public void WriteErrorLine(string value, params object[] args)
        {
            using (StreamWriter wtr = new StreamWriter(Context.Response.OutputStream))
                wtr.Write("<Error>" + value + "</Error>", args);
        }

        protected virtual bool ParseQueryString() => true;

        protected abstract void HandleRequest();
    }

    //internal class RequestHandlers
    //{
    //    internal static readonly Dictionary<string, RequestHandler> Handlers = new Dictionary<string, RequestHandler>
    //    {
    //        {"/crossdomain.xml", new Crossdomain()},
    //        {"/mysterybox/getBoxes", new mysterybox.getBoxes()},
    //        {"/package/getPackages", new packages.getPackages()},
    //        //{"/arena/getPersonalBest", new ArenaPersonalBest()},
    //        //{"/arena/getRecords", new ArenaRecords()},
    //        {"/app/globalNews", new app.globalNews()},
    //        {"/app/getLanguageStrings", new app.languageSettings()},
    //        {"/app/init", new app.init()},
    //        //{"/clientError/add", new Add()},
    //        {"/account/purchaseSkin", new account.purchaseSkin()},
    //        {"/account/verifyage", new account.verifyage()},
    //        // /account/getCredits
    //        {"/account/purchasePackage", new account.purchasePackage()},
    //        {"/playerMuledump/view", new playerMuledump.view()},
    //        {"/account/acceptTOS", new account.acceptTOS()},
    //        // /account/getBeginnerPackageTimeLeft
    //        // /getoffers
    //        // /kabam/getcredentials
    //        // /account/sendVerifyEmail
    //        // /steamworks/purchaseOffer
    //        // /steamworks/register
    //        // /kongregate/getcredentials
    //        // /kongregate/register
    //        // /kongregate/internalRegister
    //        // /steamworks/getcredentials
    //        {"/account/playFortuneGame", new account.playFortuneGame()},
    //        {"/account/redeemGiftCode", new account.redeemGiftCode()},
    //        {"/account/checkGiftCode", new account.checkGiftCode()},
    //        {"/account/resetPassword", new account.resetPassword()},
    //        {"/account/validateEmail", new account.validateEmail()},
    //        {"/account/changeEmail", new account.changeEmail()},
    //        {"/account/purchaseMysteryBox", new account.purchaseMysteryBox()},
    //        {"/account/getProdAccount", new account.getProdAccount()},
    //        {"/account/register", new account.register()},
    //        {"/account/verify", new account.verify()},
    //        {"/account/forgotPassword", new account.forgotPassword()},
    //        {"/account/sendVerifyEmail", new account.sendVerifyEmail()},
    //        {"/account/changePassword", new account.changePassword()},
    //        {"/account/purchaseCharSlot", new account.purchaseCharSlot()},
    //        {"/account/setName", new account.setName()},
    //        {"/char/list", new @char.list()},
    //        {"/char/delete", new @char.delete()},
    //        {"/char/fame", new @char.fame()},
    //        {"/credits/getoffers", new credits.getoffers()},
    //        {"/credits/add", new credits.add()},
    //        {"/credits/kabamadd", new credits.kabamadd()},
    //        {"/char/purchaseClassUnlock", new @char.purchaseClassUnlock()},
    //        {"/fame/list", new fame.list()},
    //        {"/picture/get", new picture.get()},
    //        {"/guild/getBoard", new guild.getBoard()},
    //        {"/guild/setBoard", new guild.setBoard()},
    //        {"/guild/listMembers", new guild.listMembers()}
    //    };
    //}
}