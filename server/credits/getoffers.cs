#region

using System.Net;
using System.Text;

#endregion

namespace server.credits
{
    internal class getoffers : RequestHandler
    {
        protected override void HandleRequest()
        {
            byte[] res = Encoding.UTF8.GetBytes(
                "<Offers><Tok>WUT</Tok><Exp>STH</Exp><Offer><Id>0</Id><Price>1</Price><RealmGold>10</RealmGold><CheckoutJWT>1</CheckoutJWT><Data>YO</Data><Currency>HKD</Currency></Offer><Offer><Id>1</Id><Price>5</Price><RealmGold>60</RealmGold><CheckoutJWT>60</CheckoutJWT><Data>YO</Data><Currency>HKD</Currency></Offer><Offer><Id>2</Id><Price>10</Price><RealmGold>1200</RealmGold><CheckoutJWT>10</CheckoutJWT><Data>YO</Data><Currency>HKD</Currency></Offer><Offer><Id>3</Id><Price>15</Price><RealmGold>2000</RealmGold><CheckoutJWT>15</CheckoutJWT><Data>YO</Data><Currency>HKD</Currency></Offer><Offer><Id>4</Id><Price>20</Price><RealmGold>3000</RealmGold><CheckoutJWT>20</CheckoutJWT><Data>YO</Data><Currency>HKD</Currency></Offer></Offers>");
            Context.Response.OutputStream.Write(res, 0, res.Length);
        }
    }
}