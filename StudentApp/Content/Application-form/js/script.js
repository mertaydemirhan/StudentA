const links = document.querySelectorAll(".links li");
const pages = document.querySelectorAll("form .page");
const nextBtn = document.getElementById("next");
const backBtn = document.getElementById("back");
const submitBtn = document.getElementById("submit");
const btns = document.querySelectorAll("form .btn");
const form = document.querySelector("form");

var currentPage = 1;

const changeCurrentPage = (value, link = false) => {
    if (value == "equalize") {
        for (let i = 0; i < pages.length; i++) {
            pages[i].classList.remove("active");
        }
        pages.forEach((page) => {
            if (page.id == link.id) {
                currentPage = parseInt(page.id);
                console.log(currentPage);
                page.classList.add("active");
            }
        });
        //
        for (let i = 0; i < links.length; i++) {
            links[i].classList.remove("active");
        }
        link.classList.add("active");
    } else {
        for (let i = 0; i < pages.length; i++) {
            if (parseInt(pages[i].id) == currentPage) {
                pages[i].classList.remove("active");
            }
        }

        for (let i = 0; i < links.length; i++) {
            links[i].classList.remove("active");
        }

        if (value == "increment") {
            currentPage += 1;
        } else if (value == "decrement") {
            currentPage -= 1;
        }

        pages.forEach((page) => {
            if (parseInt(page.id) == currentPage) {
                page.classList.add("active");
            }
        });
        links.forEach((link) => {
            if (parseInt(link.id) == currentPage) {
                link.classList.add("active");
            }
        });
    }

    // Check if there should be NEXT or PREVIOUS button
    let className = "active";
    if (currentPage == 1) {
        nextBtn.classList.add(className);
        backBtn.classList.remove(className);
        submitBtn.classList.remove(className);
    } else if (currentPage == 6) {
        nextBtn.classList.remove(className);
        backBtn.classList.add(className);
        submitBtn.classList.add(className);
    } else {
        nextBtn.classList.add(className);
        backBtn.classList.add(className);
        submitBtn.classList.remove(className);
    }
};

form.addEventListener("submit", (e) => {
    
    if (e.submitter.id != "submit") {
        e.preventDefault();
    }

    // Input validation
    let isAllRequiredFilled = true;
    let activeRequiredDivs = document.querySelectorAll(
        ".page.active .required"
    );

    activeRequiredDivs.forEach((div) => {
        let element = div.childNodes[3];

        // Text inputs, date inputs, select options
        if (
            (element.tagName == "INPUT" && element.value.trim() == "") ||
            (element.tagName == "SELECT" && element.value == "Select")
        ) {
            element.parentElement.classList.add("wrong");
            isAllRequiredFilled = false;
        } else {
            element.parentElement.classList.remove("wrong");
        }
    });

    links.forEach((link) => {
        if (link.id == currentPage) {
            if (isAllRequiredFilled) {
                link.classList.add("correct");
                link.classList.remove("wrong");
            } else {
                link.classList.add("wrong");
                link.classList.remove("correct");
            }
        }
    });

    if (e.submitter.id == "next") {
        changeCurrentPage("increment");
    } else if (e.submitter.id == "back") {
        changeCurrentPage("decrement");
    }
});

links.forEach((link) => {
    link.addEventListener("click", () => {
        changeCurrentPage("equalize", link);
    });
});

// Languages
let inputToggle = document.querySelector(".lang-checkbox");
let languageContainer = document.querySelector(".languages");
let langDropdown = document.querySelector(".lang-dropdown");

inputToggle.addEventListener("change", () => {
    if (inputToggle.checked) {
        languageContainer.classList.add("active");
        langDropdown.parentElement.classList.add("required");
    } else {
        languageContainer.classList.remove("active");
        langDropdown.parentElement.classList.remove("required");
    }
});

langDropdown.addEventListener("change", () => {
    let id = langDropdown.options[langDropdown.selectedIndex].id;

    document.querySelectorAll(".lang").forEach((div) => {
        div.classList.remove("active");
    });

    document.querySelector(`.lang.${id}`).classList.add("active");
});

