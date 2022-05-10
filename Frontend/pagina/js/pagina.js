function selectSim() {
    sim = document.getElementById("op").value;

    if (sim == "---") {
        alert("Simulação não selecionada");
        document.getElementById("ref").href = "#";
    }
    if (sim == "Simulação 1") {
        document.getElementById("ref").href = "sim.html";
    }
}