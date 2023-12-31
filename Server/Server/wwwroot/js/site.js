﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.

//  JS for balls background
let bg = document.getElementById('background');
let isFirstChip = true; // Flag variable to track the first chip creation

function createChip(color) {
    let chip = document.createElement('div');
    chip.className = `chip ${color}`;
    chip.style.left = Math.random() * bg.offsetWidth + 'px';
    chip.style.visibility = 'hidden'; // Set initial visibility to hidden
    bg.appendChild(chip);

    if (!isFirstChip) {
        // Calculate random animation delay for non-first chips
        let delay = Math.random() * 5;
        chip.style.animationDelay = `${delay}s`;

        // Show the chip just before the animation starts
        setTimeout(function () {
            chip.style.visibility = 'visible';
        }, delay * 1000); // Multiply delay by 1000 to convert to milliseconds
    } else {
        // Show the first chip immediately without a delay
        chip.style.visibility = 'visible';
        isFirstChip = false;
    }

    // Remove the chip when the animation ends
    chip.addEventListener('animationend', function () {
        bg.removeChild(chip);
    });
}

// Create chips at regular intervals
setInterval(function () {
    createChip('red');
    createChip('gold');
}, 2000); // Change this value to adjust the frequency of chip creation

