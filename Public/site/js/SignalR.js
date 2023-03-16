
(function ($) {
	'use strict';


	// Live Chat 
	$(function () {
		var messagesContainer = $('.chat');
		function currentTime() {
			var date = new Date();
			var localeSpecificTime = date.toLocaleTimeString();
			return localeSpecificTime.replace(/:\d+ /, ' ');
		}
		var msgTime = currentTime();
		// Reference the auto-generated proxy for the hub.
		var chat = $.connection.chatHub;
		// Create a function that the hub can call back to display messages.
		chat.client.addNewMessageToPage = function (name, message) {
			// Add the message to the page.
			var currTime = new Date();
			//var msgTime = currTime.getHours() + ":" + currTime.getMinutes();


			var messageLi =
				`
				<li class="you">
					<div class="entete">
						<span class="status"></span>
						<h2>` + htmlEncode(name) + `</h2>
						<h3>`+ msgTime + `</h3>
					</div>
					<div class="triangle"></div>
					<div class="message">`+ htmlEncode(message) + `</div>
				</li>
			`;
			messagesContainer.append([messageLi].join(''));
			messagesContainer.finish().animate({
				scrollTop: messagesContainer.prop("scrollHeight")
			}, 250);
		};
		// Set initial focus to message input box.
		$('#message').focus();
		// Start the connection.
		$.connection.hub.start().done(function () {
			$('#sendmessage').click(function () {
				if ($('#message').val() != null && $.trim($('#message').val())) {
					var msgName = $('#displayname').val();
					var msg = $('#message').val();
					var time = msgTime;
					//var time = $('#LiveChatMsgTime').text();

					chat.server.send(msgName, msg);
					// Clear text box and reset focus for next comment.
					$('#message').val('').focus();
					console.log(time);
					$.ajax({

						url: "/lesson/createmsg",
						data: {
							'name': msgName,
							'message': msg,
							'msgtime': time
						},
						cache: false,
						type: "Post",
						success: function (response) {
							console.log(response);
						}

					})
				}
			});
		});


		var shiftDown = false;
		var messageBox = $('#message');
		$(document).keypress(function (e) {
			if (e.keyCode == 13) {
				if (messageBox.is(":focus") && !shiftDown) {
					e.preventDefault(); // prevent another \n from being entered
					$("#sendmessage").trigger('click')
				}
			}
		});

		$(document).keydown(function (e) {
			if (e.keyCode == 16) shiftDown = true;
		});

		$(document).keyup(function (e) {
			if (e.keyCode == 16) shiftDown = false;
		});

	});



	// This optional function html-encodes messages for display in the page.
	function htmlEncode(value) {
		var encodedValue = $('<div />').text(value).html();
		return encodedValue;
	}


})(jQuery);