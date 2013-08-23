using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class DecodePolyline : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        List<Location> s=DecodePolylinePoints("e`miGhmocNcN~DgBiNc@uEkFik@oNg~@aIam@sm@lHct@p]o}@wFsv@gn@aDy_A}_@k~@y^uIo[a`@kNqCySlNgcArQ}nB~Og~AbTy\\GiCmS_PuuDii@evDks@evF_p@ssBye@{bGbAuz@vUo~A}q@ozAi|A}tCs|By}Fgn@khBeOioAqg@srDafAkdIrBmtCyG_yCqm@_cE_b@a`C}_@snCcDszAha@e}At]{v@xD}e@yf@aeIm^{rEgp@ahBiZu`BkVueHel@qjFahGcfe@csAeaKgUcgFkeAenIaNgwArTmgArBwmAoPe`Be`AqcDyFs`Byu@}aCaO}xA|CydAbKagBfYi`AeMctCyv@}|Gem@spIic@acF_r@czEg}@yxCuf@gwB_o@cbAah@smE{sAavBkn@{yF_s@qcCg}@acB_v@i~D{s@meCs@gaAgCyhBqSgk@sX_m@aN_`@mdDs~Iyd@g{AwpA_yH{a@csC_mAwqHab@izB}v@{wCg{@s{Fgb@ehDzKsdAxO}vAiRmpCifAkdOyyA{uS}n@uoI_[uoAo]_sAqOmo@rB_yB{d@gjCiOw`Fk`@sfSod@iyWsPamKaIep@h[ubAtk@q~BlJwlA`f@e_D\aeCyq@_pBe}@wxFcf@ypAsn@wm@kmAocD_W}mBeYkyDaUejHw_@{rI_VoaBms@gkGaFon@q@muA~G{p@H_sAmt@q~DcxCe`KaMijB|BkmCeYauBmTmzA{h@_cAkf@aaAoMiq@wnAuqBi\\}Zug@{iAky@o|@uvBon@ucC_kBoyFs~Fcb@qdDzH{hAwa@c{AkXeh@oQkFk{AHqj@qa@cfB_oCcxA}y@apAuwC_pCijEqwAwmCyj@oj@e}C}mEuzGuoIaaAeyDoeEegHsl@i_AckAqq@}f@kd@av@{vAsaLs{S}~DirJezKgp^ajAisDs^{Qit@ePoPcX_`B}pF{Qsn@mmAmcEwaHiyVqi@evCwNa`Blu@c}Cjb@gvBlIg~CwFy`Emw@a{Ceb@_lJecDcnKgjGooPuaAmxAqpCyhJo|AemFee@{XeVii@s{CoiHwd@}bC{xEa~Lix@}mBoqCskD{d@iq@}nAkbD_lFm~Pws@s|AyrBy{Ckr@ohBwDmbBuU{`BqWm`AmYcYaj@ee@yRgv@ucB}~G_l@}iGk`Ak`Gkh@oiCdH{iHgFacAxCicGrOafDrPmjB}u@alBmbBozD}i@_^_UmIcm@gcAcu@ulAqJy[yYiQwh@y[oUkVw@}OzHxF");
    }

    private List<Location> DecodePolylinePoints(string encodedPoints)
    {
        if (encodedPoints == null || encodedPoints == "") return null;
        List<Location> poly = new List<Location>();
        char[] polylinechars = encodedPoints.ToCharArray();
        int index = 0;

        int currentLat = 0;
        int currentLng = 0;
        int next5bits;
        int sum;
        int shifter;

        try
        {
            while (index < polylinechars.Length)
            {
                // calculate next latitude
                sum = 0;
                shifter = 0;
                do
                {
                    next5bits = (int)polylinechars[index++] - 63;
                    sum |= (next5bits & 31) << shifter;
                    shifter += 5;
                } while (next5bits >= 32 && index < polylinechars.Length);

                if (index >= polylinechars.Length)
                    break;

                currentLat += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);

                //calculate next longitude
                sum = 0;
                shifter = 0;
                do
                {
                    next5bits = (int)polylinechars[index++] - 63;
                    sum |= (next5bits & 31) << shifter;
                    shifter += 5;
                } while (next5bits >= 32 && index < polylinechars.Length);

                if (index >= polylinechars.Length && next5bits >= 32)
                    break;

                currentLng += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);
                Location p = new Location();
                p.Latitude = Convert.ToDouble(currentLat) / 100000.0;
                p.Longitude = Convert.ToDouble(currentLng) / 100000.0;
                poly.Add(p);
            }
        }
        catch (Exception ex)
        {
            // logo it
        }
        return poly;
    }
}

public class Location
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}
