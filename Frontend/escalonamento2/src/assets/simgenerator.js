function createTable(rn, cn)
{
  for(var r=0;r<parseInt(rn,10);r++)
    {
      var x=document.getElementById('myTable').insertRow(r);
      for(var c=0;c<parseInt(cn,10);c++)
      {
        var y=  x.insertCell(c);

        const newinput = document.createElement("input");

        newinput.setAttribute("type", "text");
        newinput.style = "width: 15px"

        y.appendChild(newinput);
      }
    }
}
