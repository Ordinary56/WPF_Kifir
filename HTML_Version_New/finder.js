"use strict";
//@ts-check

/** @typedef {import("./main.js").Student} Student */

/** @type {Student[]} */
import { data } from "./main.js";


/**
 * @param {Student[]} students 
 * @returns {void}
 */
const DisplayStudent = (students) => {
    const result_Table = document.getElementById("result_table");
    result_Table.children[1].textContent = '';
    students.forEach(student => {
        let row = document.createElement("tr");
        let data;
        for (let key in student) {
            data = document.createElement("td");
            data.innerText = student[key];
            row.appendChild(data)
        }
        result_Table.children[1].appendChild(row);
    });
    const Math_average = (students.reduce((acc, current) => acc + current.Matematika, 0) / students.length).toFixed(1);
    const Hungarian_average = (students.reduce((acc, current) => acc + current.Magyar, 0) / students.length).toFixed(1);
    document.getElementById("result_span").innerText += `\nMatematika átlaga: ${Math_average}\nMagyar átlaga: ${Hungarian_average}`;
};


/**
 * 
 * @param {string} input 
 * @returns {void} 
 */
const FindStudent = (input) => {
    /**
     * @type {Student[]|undefined}
     */
    let students;
    students = input.length == 0 ? data.sort((a, b) => a.Neve.localeCompare(b.Neve)) : data.filter(x => x.OM_Azonosito.startsWith(input)).sort((a, b) => a.Neve.localeCompare(b.Neve));
    document.getElementById("result_span").textContent = `${students.length} db találat`;
    DisplayStudent(students);
}
document.getElementById("OM_Input").addEventListener("input", (e) => FindStudent(e.target.value));
FindStudent("");