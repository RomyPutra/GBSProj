<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:template match="/">
	  <html>
		  <head>
			  <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
				  <title>Your AirAsia booking has been confirmed</title>
				  <style type="text/css">
					  body {
					  font-size:11px;
					  margin-left: 0px;
					  margin-top: 0px;
					  margin-right: 0px;
					  margin-bottom: 0px;
					  }
				  </style>
			  </meta >
		  </head>

		  <body>
			  <div style="background-color:#f3f3f3;">
			<table style="background-color:#f3f3f3;" width="100%"><tr><td align="center" style="color:#000001">
			<table width="750" border="0" align="center" cellpadding="0" cellspacing="0" style="font-family:Arial,Helvetica,sans-serif;">
  <tr>
    <td align="left"><a href="http://www.airasia.com" target="_blank"><img src="https://gbs.airasia.com/GBS/Images/EmailTemplate/logo.gif" alt="AirAsia" border="0" style="display:block" /></a></td>
    <td align="right" valign="bottom" style="font-family:Arial,Helvetica,sans-serif;"><span style="font-size:12px"><br />
      </span>
      <table border="0" align="right" cellpadding="0" cellspacing="0" style="font-family:Arial,Helvetica,sans-serif;">
        <tr>
          <td height="70" align="right" valign="top" style="color:#000001"><span style="font-size:12px">Your booking number is:<br />
            <span style="font-size:36px">{PNR}</span></span></td>
        </tr>
      </table></td>
  </tr>
</table>
<table width="776" align="center" cellpadding="0" cellspacing="0" style="border:#cccccc thin solid;">
  <tr>
    <td><table width="776" border="0" align="center" cellpadding="0" cellspacing="0" style="background-color:#cccccc">
      <tr>
        <td style="background-color:#ffffff">
          <br />
          <table width="680" border="0" align="center" bgcolor="#FFFFFF" cellpadding="0" cellspacing="0">
            <tr>
              <td colspan="2" align="left" valign="top" style="font-family:Arial, Helvetica, sans-serif;"><span style="font-size:13px">Dear {Title} {FirstName} {LastName},</span><br /></td>
            </tr>
          </table>
          <br />
          <table width="680" border="0" align="center" cellpadding="0" cellspacing="0" style="font-family:Arial, Helvetica, sans-serif;">
            <tr>
              <td height="70" align="left" valign="top" style="font-size:13px"><span style="font-family:Arial, Helvetica, sans-serif;">Thank you for completing the payment for:</span><br />
                <br />
                <table width="100%" border="0" cellspacing="1" cellpadding="0">
                  <tr>
                    <td width="5%" align="left" valign="middle" style="font-family:Arial, Helvetica, sans-serif;"><span style="font-size: 27px;"><img border="0" src="https://gbs.airasia.com/GBS/Images/EmailTemplate/confirm.gif" /></span></td>
                    <td width="33%" align="left" valign="middle" style="font-family:Arial, Helvetica, sans-serif;"><p><span style="font-size: 27px;"> <font color="#66cc00"><strong>CONFIRMED</strong></font></span><span style="font-size:20px; color:#d00; font-weight:bold;"><br />
                      </span></p></td>
                    <td width="62%" style="font-family:Arial, Helvetica, sans-serif;">

                      <table><tr><td colspan="2"><span style="font-size:20px;"><strong>Transaction no: </strong></span> <span style="font-size:20px;"><font color="#dd0000"><strong>{TransID}</strong></font></span></td></tr>
					  <tr><td valign="top" width="120px"><span style="font-size:20px;"><strong>Booking no: </strong></span> </td>
					  <td>
					  <span style="font-size:20px;"><font color="#dd0000"><strong>{PNR}</strong></font></span>
					  </td></tr></table>
					  </td>
                    </tr>
              </table>
                <br />
                <a style="color:#dd0000" href="linkAgentHref">Click here</a> to complete the guest list info.<br />
                <br />
Thank you. <br /><br /><br /></td>
              </tr>
          </table></td>
      </tr>
    </table></td>
  </tr>
  <tr></tr>
</table>
<br />
<table cellspacing="0" cellpadding="0" border="0" align="center" width="750">
  <tbody>
    <tr>
      <td align="left" width="678"><table cellspacing="0" border="0" width="90">
        <tbody>
          <tr>
            <td align="left" width="45"><a title="Book Now" href="linkFacebookHref" target="_blank"><img height="38" border="0" width="38" style="display: block;" alt="Facebook" src="https://gbs.airasia.com/GBS/Images/EmailTemplate/fblogo.gif" /></a></td>
            <td align="left"><a title="Book Now" href="linkTwitterHref" target="_blank"><img height="38" border="0" width="38" style="display: block;" alt="Twitter" src="https://gbs.airasia.com/GBS/Images/EmailTemplate/twitlogo.gif" /></a></td>
          </tr>
        </tbody>
      </table>
        <table cellspacing="0" cellpadding="0" border="0" width="76%" style="color:#666">
          <tbody>
            <tr>
              <td height="21" align="left" style="font-size:11px"><span style="font-family:Arial,sans-serif;"> Click here to view the benefits of <a target="_blank" href="linkMembershipHref" style="color:#666666"> AirAsia membership</a>. Need help? <a target="_blank" href="linkAskHref" style="color:#666666">Ask AirAsia</a></span><br /></td>
            </tr>
            <tr>
              <td align="left" style="font-size:11px"><span style="font-family:Arial,sans-serif;">{Copyright}</span><span style="font-family:Arial,san-serif;"> 2013 AirAsia Berhad. All rights reserved.</span></td>
            </tr>
          </tbody>
        </table></td>
      <td align="right" width="72"><a href="http://www.airasia.com" target="_blank"><img border="0" style="display: block;" alt="AirAsia -  World's Best Low-Cost Airline" src="https://gbs.airasia.com/GBS/Images/EmailTemplate/worldbest.gif" /></a></td>
    </tr>
  </tbody>
</table>
	</td></tr></table></div>
		  </body>
	  </html>

  </xsl:template>
</xsl:stylesheet>
