function HandleGoogleApiLibrary(){
    gapi.load('client:auth2',  {
        callback: function() {
            // Initialize client & auth libraries
            gapi.client.init({
                apiKey: document.querySelector("meta[name='google-api-key']").content,
                clientId: document.querySelector("meta[name='google-client-id']").content,
                scope: 'https://www.googleapis.com/auth/userinfo.profile https://www.googleapis.com/auth/userinfo.email https://www.googleapis.com/auth/plus.me'
            }).then(
                function(success) {
                    // Libraries are initialized successfully
                    // You can now make API calls
                },
                function(error) {
                    // Error occurred
                    console.log(error) //to find the reason
                }
            );
        },
        onerror: function() {
            // Failed to load libraries
        }
    });
}
document.getElementById("google-login").addEventListener("click", (event) => {
    event.preventDefault();
    gapi.auth2.getAuthInstance().signIn().then(
        function(success) {
            // Login API call is successful	
            console.log(success);
            let token = success.tc.id_token;
            document.getElementById("token-input").value = token;
            document.forms["external"].submit();
        },
        function(error) {
            // Error occurred
            console.log(error)//to find the reason
        }
    );
});