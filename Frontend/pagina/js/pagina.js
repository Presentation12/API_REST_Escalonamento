function selectSim() {
    sim = document.getElementById("op").value;

    if (tipoPlano == "---") {
        alert("Simulação não selecionada");
        document.getElementById("ref").href = "#";
    }
    if (tipoPlano == "Simulação 1") {
    document.getElementById("ref").href = "pagina.html";
    }
}