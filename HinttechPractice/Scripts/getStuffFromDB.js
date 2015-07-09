function showHolidays()
{
var xmlhttp;

if (window.XMLHttpRequest)
  {
    xmlhttp = new XMLHttpRequest();
  }
else
  {
    xmlhttp=new ActiveXObject("Microsoft.XMLHTTP");
  }


xmlhttp.onreadystatechange=function()
{
    
    var jsonResponse;
     if (xmlhttp.readyState==4 && xmlhttp.status==200)
    {
         jsonResponse = JSON.parse(xmlhttp.responseText);
         console.log(jsonResponse);
    }
  }
    xmlhttp.open("GET", "../Scripts/asp/getHoliday.asp", true);
    xmlhttp.send();
    
}