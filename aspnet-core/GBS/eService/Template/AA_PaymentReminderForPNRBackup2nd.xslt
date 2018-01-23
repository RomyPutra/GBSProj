<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:template match="/">
	  <html>
		  <head>
			  <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
				  <title>Air Asia Payment Reminder</title>
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
			<table style="background-color:#f3f3f3;" width="100%"><tr><td align="center"><span type="dynamic" name="header">
			<table align="center" border="0" cellpadding="0" cellspacing="0" width="776">
        <tbody><tr>
          <td height="99"><a href="http://www.airasia.com" target="_blank"><img src="https://gbs.airasia.com/GBS/Images/EmailTemplate/logo.gif" border="0" height="124" width="154" /></a></td>
          <td align="right" valign="bottom">
          </td>
        </tr>
      </tbody></table></span><table style="border:#cccccc thin solid" align="center" cellpadding="0" cellspacing="0" width="776">
      <tbody><tr>
        <td style="font-size:14px">
          <table style="background-color:#ffffff;" align="center" border="0" cellpadding="0" cellspacing="20" width="776">
            <tbody><tr>
					  <td style="font-size:14px">
						  <p><span style="font-family:Arial,sans-serif;">Dear {Title} {FirstName} {LastName}, </span></p>
					  </td>
				  </tr>
				  <tr>
					  <td style="font-size:14px">
						  <p><span style="font-family:Arial,sans-serif;">This is a payment reminder for<br/> Transaction No: <b> {TransID} </b><br/> Booking No: <b> {PNR} </b> </span></p>
					  </td>
				  </tr>
				  <tr>
					  <td height="10" style="font-size:14px">
						  <p><span style="font-family:Arial,sans-serif;">
						     There is an outstanding balance payment of <b> {Currency} {BalanceDue} </b> {Remark} which will be due on <b> {ExpiryDate} </b>.<br/>
						     Please arrange payment before the due date or if you cannot make payment on time,
						     please contact our Group Booking Team for further processing.<br/>
							 </span>
						  </p>
					  </td>
				  </tr>
				   <tr>
					  <td height="10" style="font-size:14px"><p><span style="font-family:Arial,sans-serif;">Fail in clearing the outstanding will cause the transaction forfeited.</span></p>
							  <p><span style="font-family:Arial,sans-serif;">If payment has recently been made, please accept our thanks and ignore this reminder or
							      contact our Group Booking Team for clarification.</span></p></td>
				  </tr>
				  <tr>
					  <td style="font-size:14px">
						  <p>
						 <span style="font-family:Arial,sans-serif;">Kindly click on to this link <a href="http://172.30.2.253/GBS" target="_blank"> http://www.airasia.com/GBS</a> to Login and make payment. Thank you.  <br/></span>

						  </p>
						
					  </td>
				  </tr>

				  <tr>
					  <td style="font-size:14px">
						<span style="font-family:Arial,sans-serif;">
						 Kind regards,<br/>
						 AirAsia BHD
						 </span>
					  </td>
				  </tr>
                </tbody></table></td>
            </tr>
          </tbody></table>
		  <br/>
		  <table align="center" border="0" cellpadding="0" cellspacing="0" width="750">
      <tbody><tr>
        <td align="left" width="678">
          <table border="0" cellspacing="0" width="90">
            <tbody><tr>
              <td align="left" width="45"><a href="http://www.facebook.com/AirAsia" title="Book Now" target="_blank"><img src="https://gbs.airasia.com/GBS/Images/EmailTemplate/fblogo.gif" border="0" height="38" width="38" /></a></td>
              <td align="left"><a href="http://twitter.com/AirAsia" title="Book Now" target="_blank"><img src="https://gbs.airasia.com/GBS/Images/EmailTemplate/twitlogo.gif" border="0" height="38" width="38" /></a></td>
            </tr>
          </tbody></table>
          <table border="0" cellpadding="0" cellspacing="0" width="76%" style="color:#666">
            <tbody><tr>
              <td align="left" height="21" style="font-size:10.5px"><span style="font-family:Arial,sans-serif;"><a href="http://www.airasia.com/my/en/tnc_main.html" style="color:#666" target="_blank">Terms and conditions</a> apply. Click here to read our <a href="http://www.airasia.com/my/en/friendlyreminder.page" style="color:#666;" target="_blank"> Friendly reminders</a>. Need help? <a href="http://www.airasia.com/ask" style="color:#666;" target="_blank">Ask AirAsia</a><br /></span>
</td>
            </tr>
            <tr>
              <td align="left" style="font-size:10.5px"><span style="font-family:Arial,sans-serif;">Copyright 2012 AirAsia Berhad. All rights reserved.<br /></span></td>
            </tr>
          </tbody></table>
        </td>
        <td align="right" width="72"><a href="http://www.airasia.com" target="_blank"><img src="https://gbs.airasia.com/GBS/Images/EmailTemplate/worldbest.gif" alt="AirAsia -  World&#39;s Best Low-Cost Airline 2009, 2010, 2011, 2012" style="display:block" border="0" /></a><br /></td>
      </tr>
    </tbody></table>
	</td></tr></table></div>
		  </body>
	  </html>

  </xsl:template>
</xsl:stylesheet>
