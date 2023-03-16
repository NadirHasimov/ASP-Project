(function () {
	"use strict";

    function showPageLoadingSpinner() {
        $(".overlay").addClass("overlayActive");
    }

    //CREATING QUESTION AND ANSWERS 
    $("#NewQuestionCreateButton").click(function () {
        
        var NewQuestionRow = $("#NewQuestionRow").val();
        var NewQuestionRightAnswer = $('#NewQuestionRightAnswer').find(":selected").val();
        var NewQuestionDescription = $("#NewQuestionDescription").val();
        var NewQuestionExamId = $("#NewQuestionExamId").val();

        var data =
        {
            'ExamId': NewQuestionExamId,
            'TrueAnswer': NewQuestionRightAnswer,
            'Row': NewQuestionRow,
            'Description': NewQuestionDescription
        };

        if (NewQuestionRow.length != 0 && NewQuestionRightAnswer != "Empty" && NewQuestionExamId.length != 0) {
            showPageLoadingSpinner();
            $("#errorLink").remove();
            $.ajax({
                type: "POST",
                url: "/admin/questions/addQuestion",
                data: data
            }).done(function () {
                location.reload();
            }).fail(function () {
                alert("error");
            });
        }
        else {
            $("#errorLink").remove();
            var errorLink = '<span id="errorLink" style="color:red">Zəhmət olmasa boş yerləri doldurun.</span>';
            $(".overlay ").parent().append(errorLink);
        }
    });



	var treeviewMenu = $('.app-menu');

	// Toggle Sidebar
	$('[data-toggle="sidebar"]').click(function(event) {
		event.preventDefault();
		$('.app').toggleClass('sidenav-toggled');
	});

	// Activate sidebar treeview toggle
	$("[data-toggle='treeview']").click(function(event) {
		event.preventDefault();
		if(!$(this).parent().hasClass('is-expanded')) {
			treeviewMenu.find("[data-toggle='treeview']").parent().removeClass('is-expanded');
		}
		$(this).parent().toggleClass('is-expanded');
	});

	// Set initial active toggle
	$("[data-toggle='treeview.'].is-expanded").parent().toggleClass('is-expanded');

	//Activate bootstrip tooltips
	$("[data-toggle='tooltip']").tooltip();

})();
