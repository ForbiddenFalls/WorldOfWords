var app = app || {};

app.word = function () {
    var catchedLetterId,
        $htmlWord,
        isVerticalPosition = false;

    var $loadEvents = function () {
        var $words = $('.word');
        $words.on('dragstart', drag);
        $words.on('mousedown', 'span', catchLetter);
        $words.on('click', rotate);
    }();

    function catchLetter(ev) {
        catchedLetterId = Number($(this).attr('data-position'));
    }

    function drag(ev) {
        ev.originalEvent.dataTransfer.setData('text/plain', 'anything');
        $htmlWord = $(this).parent();
    }

    function rotate(ev) {
        var $word = $(this);
        if ($word.hasClass('vertical')) {
            $word.removeClass('vertical');
        } else {
            $word.addClass('vertical');
        }

        isVerticalPosition = !isVerticalPosition;
    }

    function getCatchedLetterId() {
        return catchedLetterId;
    }

    function getWord() {
        var word = $($htmlWord).children(".word").text();
        word = word.replace(/[ \n\r]+/g, '');
        return word;
    }

    function getHtmlWord() {
        return $htmlWord;
    }

    function isVertical() {
        return isVerticalPosition;
    }

    function clear() {
        catchedLetterId = null;
        $htmlWord = null;
        isVerticalPosition = false;
    }

    return {
        getCatchedLetterId: getCatchedLetterId,
        getWord: getWord,
        getHtmlWord: getHtmlWord,
        isVertical: isVertical,
        clear: clear
    }
}();



app.board = function (word) {
    var words = [],
        dropCellId,
        size,
        startPosition;

    function setSize(boardSize) {
        size = boardSize;
    }


    function allowDrop(ev) {
        ev.preventDefault();
    }

    function isWordInBoard() {
        var start,
            end;

        if (word.isVertical()) {
            start = dropCellId - word.getCatchedLetterId() * size;
            end = start + (word.getWord().length - 1) * size;
            if (start < 0 || end >= size * size) {
                return false;
            }
        } else {
            start = dropCellId - word.getCatchedLetterId();
            end = start + word.getWord().length - 1;

            if (parseInt(start / size) != parseInt(end / size)) {
                return false;
            }
        }

        startPosition = start;
        return true;
    }

    function hasWord() {
        var index = words.indexOf(word.getWord());
        return index >= 0;
    }

    function pushWordInBoard() {
        var s = size,
            $cells = $('.board span'),
            wordAsText = word.getWord(),
            i;

        if (!isWordInBoard() || hasWord()) {
            return;
        }

        if (!word.isVertical()) {
            s = 1;
        }

        var currentCell;
        for (i in wordAsText) {
            currentCell = $cells[startPosition + Number(i) * s];
            if (currentCell.innerHTML !== wordAsText[i] && currentCell.innerHTML !== '&nbsp;') {
                return;
            }
        }

        for (i in wordAsText) {
            currentCell = $cells[startPosition + Number(i) * s];
            currentCell.innerHTML = wordAsText[i];
        }

        words.push(word.getWord());
        //todo save in database with AJAX
        console.log(getBoardAsJson());
        word.getHtmlWord().remove();
        word.clear();
    }

    function drop(ev) {
        ev.preventDefault();
        dropCellId = Number($(ev.target).attr('data-position'));
        pushWordInBoard();
    }

    function getCellId(ev) {
        dropCellId = Number($(this).attr('data-position'));
    }

    function getBoardAsJson() {
        var boardContent = $('.board').text();
        boardContent = boardContent.replace(/[ \n\r]+/g, '');
        var json = JSON.stringify({
            content: boardContent,
            words: words
        });

        return json;
    }

    function loadBoard(boardAsJson) {
        var board = JSON.parse(boardAsJson),
            content = board.content;
        words = board.words;
        var $cells = $('.board span'),
            i;
        for(i in content) {
            $cells[i].innerHTML = content[i].replace(' ', '&nbsp;');
        }
    }

    var $loadEvents = function () {
        var $board = $('.board');
        $board.on('dragover', allowDrop);
        $board.on('drop', drop);
        $board.on('mouseup', 'span', getCellId);
    }();

    return {
        setSize: setSize,
        getBoardAsJson: getBoardAsJson,
        loadBoard: loadBoard
    };
}(app.word);

app.board.setSize(5);


