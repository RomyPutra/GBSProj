<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:template match="/">
	  <html>
		  <head>
			  <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
				  <title>Payment reminder for your AirAsia booking</title>
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
    <td align="left" style="font-family:Arial,Helvetica,sans-serif;"><a href="http://www.airasia.com" target="_blank"><img src="https://gbs.airasia.com/GBS/Images/EmailTemplate/logo.gif" alt="AirAsia" border="0" style="display:block" /></a></td>
    <td align="right" valign="bottom" style="font-family:Arial,Helvetica,sans-serif;"><span style="font-size:12px"><br />
      </span>
      <table border="0" align="right" cellpadding="0" cellspacing="0" style="font-family:Arial,Helvetica,sans-serif;">
        <tr>
          <td height="70" align="right" valign="top" style="color:#000001"><span style="font-size:12px"><span style="font-size:36px">Need payment</span></span></td>
        </tr>
      </table></td>
  </tr>
</table>
<table width="776" align="center" cellpadding="0" cellspacing="0" style="border:#cccccc thin solid;">
  <tr>
    <td style="color:#000001"><table width="776" border="0" align="center" cellpadding="0" cellspacing="0" style="background-color:#cccccc">
      <tr>
        <td style="background-color:#ffffff">
          <br />
          <table width="680" border="0" align="center" bgcolor="#ffffff" cellpadding="0" cellspacing="0" style="color:#000001">
            <tr>
              <td colspan="2" align="left" valign="top" style="font-family:Arial,Helvetica,sans-serif;">
			  <span style="font-size:13px">Dear {Title} {FirstName} {LastName},</span><br /></td>
            </tr>
          </table>
          <br />
          <table width="680" border="0" align="center" cellpadding="0" cellspacing="0" style="color:#000001">
            <tr>
              <td height="70" align="left" valign="top" style="font-family:Arial, Helvetica, sans-serif;">
<span style=" font-size:13px"><font color="#000001">			  This is a payment reminder for</font></span><br />
                <br />
                <table width="100%" border="0" cellspacing="0" cellpadding="0" style="font-family:Arial,Helvetica,sans-serif;">
                  <tr>
                    <td width="59%"><span style="font-size:20px;"><strong>Booking no: </strong></span> <span style="font-size:20px;"><font color="#dd0000"><strong>{PNR}</strong></font></span></td>
                    <td width="41%"><span style="font-size:14px;">Transaction no: </span> <span style="font-size:14px;"><strong>{TransID}</strong></span></td>
                  </tr>
                  <tr>
                    <td><span style="font-size:20px;"><strong>Outstanding payment: </strong></span> <span style="font-size:20px;"><font color="#dd0000"><strong>{Currency} {BalanceDue}</strong></font></span></td>
                    <td><span style="font-size:14px; ">Minimum payment: </span> <span style="font-size:14px;"><strong>{Currency} {MinPayment}</strong></span></td>
                  </tr>
                </table>
                <br />
                <br />
                <span style="font-size:13px"><font color="#000001">There is an outstanding balance payment which will be due on <strong>{ExpiryDate}</strong> .
Please arrange your  payment before the due date. </font><br />
<br />

<font color="#000001">Failure to clear the outstanding balance will result in your transaction being forfeited. Please ignore this reminder if payment has just been made. If you are not able to make payment on time, please contact our Group Booking Team.</font><br />
</span>
<br />
				<table style="background-color:#333333">
				<tr>
				<td style="padding:13px;">
				<a style="text-decoration:none" href="linkAgentHref">
				<span style="font-size:16px;">
				<span style="font-family:Arial,Helvetica,sans-serif;">
				<font color="#ffffff"><strong>Proceed to payment now</strong></font>
				</span>
				</span>
				</a>
				</td>
				</tr>
				</table>
				<br />
<br /><br /></td>
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
