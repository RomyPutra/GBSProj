<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:template match="/">
	  <html>
		  <head>
			  <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
				  <title>AirAsia Group Booking - Password Retrieval</title>
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
						  <p>Thank you for your request. According to our records, your login information is as follows: </p>
					  </td>
				  </tr>
				  <tr>
					  <td height="10"></td>
				  </tr>
				  <tr>
					  <td height="10">
						  <p>
						     Login Name : <font size="3"><b> {Username} </b></font><br></br>
							 Password	: <font size="3"><b> {Password} </b></font>
						  </p>
					  </td>
				  </tr>
				   <tr>
					  <td height="10"></td>
				  </tr>
				  <tr>
					  <td>
						  <p>
						 Please remember that your password is confidential, and should never be shared with others.<br></br> 
						  </p>
						  <br></br>

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
