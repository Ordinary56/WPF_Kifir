"use strict";
//@ts-check
// Valamiért Prettier így formázza a fájlt
/** @typedef {import("./main.js").Student} Student */

/** @type {Student[]} */
import { data } from "./main.js";
let students = data;
/**
 * Sorts the data by the given property
 * @param {string} direction
 * @param {string} property
 * @returns {void}
 */
const SortData = (direction, property) => {
  switch (property) {
    case "Neve":
    case "ErtesitesiCime":
    case "Email":
      students.sort((a, b) =>
        direction === "up"
          ? a[property].localeCompare(b[property])
          : b[property].localeCompare(a[property])
      );
      break;
    case "SzuletesiDatum":
      students.sort((a, b) => {
        return direction == "up"
          ? new Date(a[property]) - new Date(b[property])
          : new Date(b[property]) - new Date(a[property]);
      });
      break;
    default:
      students.sort((a, b) =>
        direction === "up"
          ? a[property] - b[property]
          : b[property] - a[property]
      );
      break;
  }
  DisplayStudent(students);
};

/**
 * 
 * @param {HTMLTableCellElement} header
 * @returns {void}
 */
const FilterByHeader = (header) => {
  switch (header.innerText) {
    case "Matek":
      SortData(header.dataset.direction, "Matematika");
      break;
    case "Magyar":
      SortData(header.dataset.direction, "Magyar");
      break;
    case "Email":
      SortData(header.dataset.direction, "Email");
      break;
    case "Neve":
      SortData(header.dataset.direction, "Neve");
      break;
    case "OM azonosító":
      SortData(header.dataset.direction, "OM_Azonosito");
      break;
    case "Értesítési Címe":
      SortData(header.dataset.direction, "ErtesitesiCime");
      break;
    case "Születési Dátum":
      SortData(header.dataset.direction, "SzuletesiDatum");
      break;
  }

  header.dataset.direction = header.dataset.direction === "up" ? "down" : "up";
};

/**
 * Displays the data in the table
 * @param {Student[]} students
 * @returns {void}
 */
const DisplayStudent = (students) => {
  const result_Table = document.getElementById("result_table");
  result_Table.children[1].textContent = "";
  document.getElementById("result_span").innerText = "";
  students.forEach((student) => {
    let row = document.createElement("tr");
    let data;
    for (let key in student) {
      data = document.createElement("td");
      data.innerText = student[key];
      row.appendChild(data);
    }
    result_Table.children[1].appendChild(row);
  });
  const Math_average = (
    students.reduce((acc, current) => acc + current.Matematika, 0) /
    students.length
  ).toFixed(1);
  const Hungarian_average = (
    students.reduce((acc, current) => acc + current.Magyar, 0) / students.length
  ).toFixed(1);
  document.getElementById(
    "result_span"
  ).innerText += `\nMatematika átlaga: ${Math_average}\nMagyar átlaga: ${Hungarian_average}`;
  
};

/**
 * Returns the number of students with this OM ID
 * @param {string} input
 * @returns {void}
 */
const FindStudent = (input) => {
  // Display all if no input is given
  students =
    input.length == 0
      ? students.sort((a, b) => a.Neve.localeCompare(b.Neve))
      : students
          .filter((x) => x.OM_Azonosito.startsWith(input))
          .sort((a, b) => a.Neve.localeCompare(b.Neve));
  document.getElementById(
    "result_span"
  ).textContent = `${students.length} db találat`;
  DisplayStudent(students);
};

document
  .getElementById("OM_Input")
  .addEventListener("input", (e) => FindStudent(e.target.value));
document.querySelectorAll("th").forEach((header) => {
  header.addEventListener("click", (e) => {
    FilterByHeader(e.target);
  });
});
FindStudent("");

