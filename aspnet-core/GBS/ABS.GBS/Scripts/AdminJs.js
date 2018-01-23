
//check date is valid date
function CheckInvalidDate(year,month,day)
{
    var tag = true;
    var LeapYear = false;

    if (year % 100 == 0) {
       if (year % 400 == 0) {
         LeapYear = true;
       }
    }

        if (month == 1 || month == 3 || month == 5 || month == 7 || month == 8 || month == 10 || month == 12) {
            if (day > 31 || day < 1) {
                alert("Invalid date.");
                tag = false;
            }
        }
        else if (month == 2) {
            if (LeapYear) {  //Leap year
                if (day > 29 || day < 1) {
                    alert("Invalid date.");
                    tag = false;
                }
            }
            else {
                if (day > 28 || day < 1) {
                    alert("Invalid date.");
                    tag = false;
                }
            }
        }
        else if (month == 4 || month == 6 || month == 9 || month == 11) {
            if (day > 30 || day < 1) {
                alert("Invalid date.");
                tag = false;
            }
        }
     return tag;
}


function CompareDate(fromYear, fromMonth, fromDay, toYear, toMonth, toDay) {
    var startDate = new Date();
    startDate.setFullYear(fromYear, fromMonth - 1, fromDay);

    var endDate = new Date();
    endDate.setFullYear(toYear, toMonth - 1, toDay);
//    alert("endDate" + endDate);

    if (!CheckInvalidDate(fromYear, fromMonth, fromDay)) {
        return false;
    }
    else if (!CheckInvalidDate(toYear, toMonth, toDay)) {
        return false;
    }
    else if(startDate > endDate){ 
      alert("Invalid date range, please confirm.");
      return false;
    }
    else {
      return true;
    }
}
