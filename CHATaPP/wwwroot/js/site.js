
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("https://localhost:44368/chat")
        .configureLogging(signalR.LogLevel.Debug)
        .build();

    connection.on("receiveMessage", (message) => {
        const msg = message.replace(/&/g, "&").replace(/</g, "<").replace(/>/g, ">");
        const encodedMsg = msg;
        const li = document.createElement("li");
        li.textContent = encodedMsg;
        document.getElementById("messagesList").appendChild(li);
    });


    connection.start().catch(err => console.error(err.toString()));

    $("#send").on("click", function () {
        const message = document.getElementById("message").value;
        const groupName = document.getElementById("groupName").value;
        const connectionId = document.getElementById("conId").value;

        if (groupName !== "") {
            connection.invoke("SendGroupMessage", groupName, message).catch(err => console.error(err.toString()));
        } else if (connectionId !== "") {
            const message = document.getElementById("message").value;
            connection.invoke("SendMessageToOneUser", connectionId, message).catch(err => console.error(err.toString()));
        }
        else {
            connection.invoke("SendMessage", message).catch(err => console.error(err.toString()));
        }

        event.preventDefault();
    })

    $("#join").on("click", function () {
        const groupName = document.getElementById("groupName").value;
        connection.invoke("JoinGroup", groupName).catch(err => console.error(err.toString()));
        event.preventDefault();
    })
