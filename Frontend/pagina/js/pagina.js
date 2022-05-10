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

document.getElementById("btnOp").addEventListener("click", (e) => {
    e.preventDefault();
    document.getElementById("novaOp").style.display = "flex";

})

document.getElementById("btnSim").addEventListener("click", (e) => {
    e.preventDefault();
    document.getElementById("simulacao").style.display = "flex";
    document.getElementById("btnSim").style.display = "none";
})

document.getElementById("btnCancelar").addEventListener("click", (e) => {
    e.preventDefault();
    document.getElementById("simulacao").style.display = "none";
    document.getElementById("btnSim").style.display = "";
})

document.getElementById("cancel").addEventListener("click", (e) => {
    e.preventDefault();
    document.getElementById("novaOp").style.display = "none";

})