let apiUrl = "https://localhost:7035/api";

let editingBirdId = null;
let editingEggId = null;

function onSaveBird() {
    let nameInput = document.getElementById("bName");
    let speciesInput = document.getElementById("bSpecies");

    let valName = nameInput.value;
    let valSpecies = speciesInput.value;

    if (valName === "" || valSpecies === "") {
        alert("Fields cannot be empty");
        return;
    }

    let data = {
        name: valName,
        species: valSpecies
    };

    if (editingBirdId === null) {
        fetch(apiUrl + "/Birds", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(data)
        })
            .then(handleResponse)
            .then(() => {
                resetBirdForm();
                getAllBirds();
            });
    } else {

        data.id = editingBirdId;

        fetch(apiUrl + "/Birds/" + editingBirdId, {
            method: "PUT",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(data)
        })
            .then(handleResponse)
            .then(() => {
                resetBirdForm();
                getAllBirds();
            });
    }
}

function getAllBirds() {
    fetch(apiUrl + "/Birds")
        .then(res => res.json())
        .then(data => {
            let tableBody = document.querySelector("#bird-table tbody");
            tableBody.innerHTML = "";

            data.forEach(item => {
                let row = `<tr>
                <td>${item.id}</td>
                <td>${item.name}</td>
                <td>${item.species}</td>
                <td>
                    <button class="btn-edit" onclick="startEditBird(${item.id}, '${item.name}', '${item.species}')">Edit</button>
                    <button class="btn-del" onclick="removeBird(${item.id})">Remove</button>
                </td>
            </tr>`;
                tableBody.innerHTML += row;
            });
        });
}

function removeBird(id) {
    if (confirm("Delete this bird?")) {
        fetch(apiUrl + "/Birds/" + id, { method: "DELETE" })
            .then(() => getAllBirds());
    }
}

function startEditBird(id, name, species) {
    editingBirdId = id;

    document.getElementById("bName").value = name;
    document.getElementById("bSpecies").value = species;

    let btn = document.getElementById("btnSaveBird");
    btn.innerText = "Update Bird";
    btn.classList.add("btn-update");

    document.getElementById("btnCancelBird").classList.remove("hidden");
}

function resetBirdForm() {
    editingBirdId = null;
    document.getElementById("bName").value = "";
    document.getElementById("bSpecies").value = "";

    let btn = document.getElementById("btnSaveBird");
    btn.innerText = "Add New Bird";
    btn.classList.remove("btn-update");

    document.getElementById("btnCancelBird").classList.add("hidden");
}


function onSaveEgg() {
    let sizeSelect = document.getElementById("eSize");
    let birdIdInput = document.getElementById("eBirdId");

    let valSize = sizeSelect.value;
    let valId = birdIdInput.value;

    if (valId === "") {
        alert("Enter Bird ID");
        return;
    }

    let data = {
        size: valSize,
        birdId: Number(valId)
    };

    if (editingEggId === null) {
        fetch(apiUrl + "/Eggs", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(data)
        })
            .then(async res => {
                if (res.ok) {
                    resetEggForm();
                    getAllEggs();
                } else {
                    let txt = await res.text();
                    alert("Error: " + txt);
                }
            });
    } else {
        data.id = editingEggId;
        fetch(apiUrl + "/Eggs/" + editingEggId, {
            method: "PUT",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(data)
        })
            .then(async res => {
                if (res.ok) {
                    resetEggForm();
                    getAllEggs();
                } else {
                    let txt = await res.text();
                    alert("Update Error: " + txt);
                }
            });
    }
}

function getAllEggs() {
    fetch(apiUrl + "/Eggs")
        .then(res => res.json())
        .then(list => {
            let container = document.querySelector("#egg-table tbody");
            container.innerHTML = "";

            list.forEach(egg => {
                let row = `<tr>
                <td>${egg.id}</td>
                <td>${egg.size}</td>
                <td>${egg.birdId}</td>
                <td>
                    <button class="btn-edit" onclick="startEditEgg(${egg.id}, '${egg.size}', ${egg.birdId})">Edit</button>
                    <button class="btn-del" onclick="removeEgg(${egg.id})">Remove</button>
                </td>
            </tr>`;
                container.innerHTML += row;
            });
        });
}

function removeEgg(eggId) {
    if (confirm("Delete egg?")) {
        fetch(apiUrl + "/Eggs/" + eggId, { method: "DELETE" })
            .then(() => getAllEggs());
    }
}

function startEditEgg(id, size, birdId) {
    editingEggId = id;

    document.getElementById("eSize").value = size;
    document.getElementById("eBirdId").value = birdId;

    let btn = document.getElementById("btnSaveEgg");
    btn.innerText = "Update Egg";
    btn.classList.add("btn-update");

    document.getElementById("btnCancelEgg").classList.remove("hidden");
}

function resetEggForm() {
    editingEggId = null;
    document.getElementById("eSize").value = "S";
    document.getElementById("eBirdId").value = "";

    let btn = document.getElementById("btnSaveEgg");
    btn.innerText = "Add New Egg";
    btn.classList.remove("btn-update");

    document.getElementById("btnCancelEgg").classList.add("hidden");
}

function handleResponse(res) {
    if (!res.ok) {
        alert("Operation failed");
        throw new Error("API Error");
    }
    return res;
}

getAllBirds();
getAllEggs();