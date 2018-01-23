<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:template match="/">
	  <html>
		  <head>
			  <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
				  <title>Air Asia Flight Credit Shell Confirmation Notice</title>
				  <style type="text/css">
					  body {
					  margin-left: 0px;
					  margin-top: 0px;
					  margin-right: 0px;
					  margin-bottom: 0px;
					  }
				  </style>
			  </meta >
		  </head>

		  <body>
			  <table width="100%" border="0" style="FONT-SIZE: 10.0pt; COLOR: black; FONT-FAMILY: Palatino Linotype , Book Antiqua, Palatino, serif">
				  <!--<tr>
            <td bgcolor="#FF0000">
              <table width="100%" border="0">
                <tr>
                  <td width="15%">
                    <img src="http://booking.airasia.com/images/AKBase/aalogoqpr.gif"  width="180" height="85"></img >
                  </td>
                  <td width="85%"></td>
                </tr>
              </table>
            </td>
          </tr>-->
				  <tr>
					  <td align="left">
						  <p>PNR No: {RecordLocator}</p>
					  </td>
				  </tr>
				  <tr>
					  <td>
						  <p>Dear Valued Guest, </p>
					  </td>
				  </tr>
				  <tr>
					  <td height="10"></td>
				  </tr>
				  <tr>
					  <td>
						  <p>Kindly be informed that we have created a credit account as per your request.  </p>
					  </td>
				  </tr>
				  <tr>
					  <td height="10"></td>
				  </tr>
				  <tr>
					  <td height="10">
						  <p>
						     Account expiry date: {ExpiryDate}
						  </p>
					  </td>
				  </tr>
				   <tr>
					  <td height="10"></td>
				  </tr>
				  <tr>
					  <td>
						  <p>
							 To utilize the credit amount, would require you to first login on the AirAsia website, and insert your user ID; which is your email address followed with your password.   <br></br>

							 Then click on the "Make a booking" tab and proceed to make your bookings. When you reach the payment page there should be four (4) Tabs for payment method as per the sample attachment.  <br></br>

							 Please select the credit account option for payment. Any balance amount may be paid via your credit Card.   <br></br>

							 Please make your bookings before the expiry date; however, your actual travel dates can be after the expiry date as long as our schedule is out. Should you have further information and inquiries, Kindly click on to this link <a href="http://www.airasia.com/my/en/contactus.html" target="_blank"> http://www.airasia.com/my/en/contactus.html</a> to contact us. Thank you.  <br></br> 

						  </p>
						  <br></br>

					  </td>
				  </tr>

				  <tr>
					  <td>
						 Kind regards,<br></br >
						 AirAsia BHD
					  </td>
				  </tr>
				  <tr>
					  <td height="20"></td>
				  </tr>
				  <tr>
					  <td  >

					  </td>
				  </tr>
			  </table>
		  </body>
	  </html>

  </xsl:template>
</xsl:stylesheet>
