<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:template match="/">
	  <html>
		  <head>
			  <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
				  <title>Air Asia Payment Reminder</title>
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
					  <td>
						  <p>Dear {Title} {FirstName} {LastName}, </p>
					  </td>
				  </tr>
				  <tr>
					  <td height="10"></td>
				  </tr>
				  <tr>
					  <td>
						  <p>This is a payment reminder for<br/> Transaction No: <b> {TransID} </b><br/> Booking No: <b> {PNR} </b> </p>
					  </td>
				  </tr>
				  <tr>
					  <td height="10"></td>
				  </tr>
				  <tr>
					  <td height="10">
						  <p>
						     There is an outstanding balance payment of <b> {Currency} {BalanceDue} </b> {Remark} which will be due on <b> {ExpiryDate} </b>.<br></br>
						     Please arrange payment before the due date or if you cannot make payment on time,
						     please contact our Group Booking Team for further processing.<br></br>
						  </p>
					  </td>
				  </tr>
				   <tr>
					  <td height="10"><p>Fail in clearing the outstanding will cause the transaction forfeited.</p>
							  <p>If payment has recently been made, please accept our thanks and ignore this reminder or
							      contact our Group Booking Team for clarification.</p></td>
				  </tr>
				<tr>
					  <td height="10"></td>
				  </tr>
				  <tr>
					  <td>
						  <p>
						 Kindly click on to this link <a href="http://172.30.2.253/GBS" target="_blank"> http://www.airasia.com/GBS</a> to Login and make payment. Thank you.  <br></br> 

						  </p>
						
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
