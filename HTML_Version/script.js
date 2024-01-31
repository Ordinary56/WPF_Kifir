"use strict";
//@ts-check

/**
 * @typedef  {Object} Student
 * @property {string} OM_ID
 * @property {string} Name
 * @property {string} Address
 * @property {string} Date
 * @property {string} Email
 * @property {number} Math
 * @property {number} Hungarian
 */

/** @type {Student[]} */
import data from "./test.json" assert {type: "json"};

// data.forEach(student => console.log(student));

/** @type {RegExp} */
const OM_Regex = /\d{11}/g;

/**
 * @param {Student} student 
 * @returns {void}
 */
const DisplayStudent = (student) => {
    const result_Table = document.getElementById("Result_Table");
    result_Table.children[0].children[0].textContent = '';
    result_Table.children[1].textContent = '';
    const fields = ["OM Azonosító", "Név", "Értesítési cím", "Email", "Születési Dátum", "Matek", "Magyar"];
    const row = document.createElement("tr");
    for (let i = 0; i < fields.length; i++) {
        let header = document.createElement("th");
        //Append headers
        header.innerText = fields[i];
        result_Table.children[0].children[0].appendChild(header);
    }
    for (let key in student) {
        let data = document.createElement("td");
        data.innerText = student[key];
        row.appendChild(data);
    }
    result_Table.children[1].appendChild(row);
}


/**
 * 
 * @param {string} input 
 * @returns {void} 
 */
const FindStudent = (input) => {
    /**
     * @type {Student|undefined}
     */
    let student;
    student = data.find(x => x.OM_Azonosito == input);
    if (student == undefined) {
        document.getElementById("Error_Span").innerText = "Ilyen tanuló ezzel az OM azonosítóval nincs!";
        return;
    }
    DisplayStudent(student);
}

document.getElementById("B_OM").addEventListener("click", (e) => FindStudent(document.getElementById("OM_Input").value));
document.getElementById("OM_Input").addEventListener("input", e => {
    document.getElementById("Error_Span").innerText = OM_Regex.test(e.target.value) ? "" : "Az OM Azonosítónak 11 számjegyből kell állnia és " +
        "nem tartalmazhat betűt";
});

