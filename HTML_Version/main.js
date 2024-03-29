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

// Megjegyzés: Firefoxon az import assertion nem működik
// Alternatíva lehet hozzá a dinamikus import vagy a fetch()
// A fetch() a natív WebAPI függvénye, tehát kliens oldalon is működik

/** @type {Student[]} */
import data from "./test.json" assert { type: "json" };
let state = "main";

/**@type {HTMLElement|null} */
const Main_Menu_Button = document.getElementById("main_menu");
const Find_Menu_Button = document.getElementById("find_menu");
console.log(Main_Menu_Button)
// Events
Main_Menu_Button.addEventListener("click", e => {
    if(state === "main") return;
    state = "main";
    document.getElementById("finder").style.visibility = "hidden";
    document.getElementById("finder").style.position = "absolute";
    document.getElementById("filter").style.position = "relative";
    document.getElementById("filter").style.visibility = "visible";
    console.log("hidden")
});
Find_Menu_Button.addEventListener("click", e => {
    if(state === "find") return;
    state = "find";
    document.getElementById("filter").style.visibility = "hidden";
    document.getElementById("filter").style.position = "absolute";
    document.getElementById("finder").style.position = "relative";
    document.getElementById("finder").style.visibility = "visible";
});
export { data };
