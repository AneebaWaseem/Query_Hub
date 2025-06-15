// site.js
"use strict";

// SignalR connection setup
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/notificationHub")
    .build();

// Answer added
connection.on("ReceiveNotification", function (username, ques, ans, id) {
    const answersContainer = document.querySelector(".px-2.mt-4");

    // Format date to "MMM dd, yyyy"
    const now = new Date();
    const formattedDate = now.toLocaleDateString('en-US', {
        month: 'short',
        day: '2-digit',
        year: 'numeric'
    });

    const newAnswerHtml = `
        <div class="border-bottom py-3" id="${id}">
            <div class="d-flex justify-content-between align-items-center flex-wrap">
                <div class="d-flex align-items-center">
                    <i class="bi bi-person-circle text-dark fs-5 me-2"></i>
                    <span class="fw-medium fs-6 mt-1">${username}</span>
                </div>
                <small class="text-muted fw-medium fs-6 mt-1">${formattedDate}</small>
            </div>
            <p class="mt-2" style="line-height: 1.6;">${ans}</p>
        </div>
    `;

    answersContainer.insertAdjacentHTML("beforeend", newAnswerHtml);
});

// Answer Deleted
connection.on("AnswerDeleted", function (answerId) {
    const el = document.getElementById(`answer-${answerId}`);
    if (el) el.remove();
});


// Start the SignalR connection
connection.start().catch(function (err) {
    return console.error(err.toString());
});

// Answer add
document.getElementById("sendButton").addEventListener("click", function (event) {
    event.preventDefault(); 

    const ans = document.getElementById("description").value;
    const ques = parseInt(document.getElementById("qId").value);

   
    connection.invoke("SendMessageToUser", ans, ques)
        .catch(function (err) {
            return console.error(err.toString());
        });

    document.getElementById("description").value = "";
});

// Answer delete
document.addEventListener("click", function (e) {
    if (e.target.classList.contains("delete-btn")) {
        const id = e.target.dataset.id;

        if (!confirm("Are you sure you want to delete this answer?")) return;

        connection.invoke("DeleteAnswer", parseInt(id))
            .catch(err => console.error(err.toString()));
    }
});

