// visualizar password
function mostraPass() {
    var x = document.getElementById("pass");
  
    if (x.type === "password") {
      x.type = "text";
    }
    else {
      x.type = "password";
    }
  }