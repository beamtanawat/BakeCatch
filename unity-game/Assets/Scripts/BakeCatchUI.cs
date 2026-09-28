using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

// Builds ordinary uGUI controls in one reference layout. Session rules live elsewhere.
public class BakeCatchUI : MonoBehaviour
{
    [SerializeField] private GameSession session;
    [SerializeField] private Font font;
    private Transform root;
    private GameObject menu, levels, instructions, hud, pause, result;
    private Text timeLabel, scoreLabel, comboLabel, levelLabel, recipeLabel, orderRows, feedback, resultStats;
    private Text leftLabel, rightLabel;
    private Image timeFill;
    private SessionState shownState;
    private readonly Color ink = BakeryArt.Hex("#603C3C");
    private readonly Color cream = BakeryArt.Hex("#FFF9EF");
    private readonly Color pink = BakeryArt.Hex("#F65D86");

    private void Start()
    {
        Canvas canvas = gameObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        CanvasScaler scaler = gameObject.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1600, 900);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
        gameObject.AddComponent<GraphicRaycaster>();
        if (EventSystem.current == null)
        {
            GameObject events = new GameObject("UI Event System", typeof(EventSystem), typeof(InputSystemUIInputModule));
            events.GetComponent<InputSystemUIInputModule>().AssignDefaultActions();
        }
        root = Rect("Layout", transform, 0, 0, 1600, 900);
        RectTransform layout = (RectTransform)root;
        layout.anchorMin = layout.anchorMax = layout.pivot = new Vector2(0.5f, 0.5f);
        layout.anchoredPosition = Vector2.zero;

        BuildMenu();
        BuildLevels();
        BuildInstructions();
        BuildHud();
        BuildPause();
        BuildResult();
        Show(menu);
        shownState = SessionState.Ready;
    }

    private RectTransform Rect(string name, Transform parent, float x, float y, float width, float height)
    {
        RectTransform rect = new GameObject(name, typeof(RectTransform)).GetComponent<RectTransform>();
        rect.SetParent(parent, false);
        rect.anchorMin = rect.anchorMax = rect.pivot = new Vector2(0, 1);
        rect.anchoredPosition = new Vector2(x, -y);
        rect.sizeDelta = new Vector2(width, height);
        return rect;
    }

    private Image Panel(Transform parent, string name, float x, float y, float width, float height, Color color)
    {
        Image image = Rect(name, parent, x, y, width, height).gameObject.AddComponent<Image>();
        image.sprite = BakeryArt.Round;
        image.type = Image.Type.Sliced;
        image.color = color;
        return image;
    }

    private Text Label(Transform parent, string value, float x, float y, float width, float height, int size, Color? color = null, TextAnchor alignment = TextAnchor.MiddleCenter)
    {
        Text text = Rect(value, parent, x, y, width, height).gameObject.AddComponent<Text>();
        text.font = font;
        text.fontSize = size;
        text.color = color ?? ink;
        text.alignment = alignment;
        text.text = value;
        text.horizontalOverflow = HorizontalWrapMode.Wrap;
        // Thai accents need more vertical font space than Latin-only labels.
        text.verticalOverflow = VerticalWrapMode.Overflow;
        text.raycastTarget = false;
        return text;
    }

    private Button Action(Transform parent, string title, float x, float y, float width, float height, UnityEngine.Events.UnityAction action, bool primary = true)
    {
        Image image = Panel(parent, title, x, y, width, height, primary ? pink : BakeryArt.Hex("#FBE3D6"));
        Button button = image.gameObject.AddComponent<Button>();
        ColorBlock colors = button.colors;
        colors.highlightedColor = BakeryArt.Hex("#FFE0A9");
        colors.selectedColor = BakeryArt.Hex("#FFE0A9");
        colors.pressedColor = BakeryArt.Hex("#E8B692");
        button.colors = colors;
        button.onClick.AddListener(action);
        Label(image.transform, title, 8, 0, width - 16, height, 26, primary ? Color.white : ink);
        return button;
    }

    private GameObject Screen(string name)
    {
        GameObject screen = Rect(name, root, 0, 0, 1600, 900).gameObject;
        screen.SetActive(false);
        return screen;
    }

    private void Header(Transform parent, string title, string subtitle)
    {
        Label(parent, "BAKE CATCH  /  THE LITTLE BAKERY", 100, 48, 1400, 35, 20);
        Label(parent, title, 160, 105, 1280, 100, 62);
        Label(parent, subtitle, 180, 207, 1240, 55, 25);
    }

    private void BuildMenu()
    {
        menu = Screen("Main Menu");
        Panel(menu.transform, "Welcome card", 330, 62, 940, 735, cream);
        Label(menu.transform, "FRESH FROM THE OVEN", 450, 103, 700, 45, 21, pink);
        Label(menu.transform, "Bake Catch", 400, 160, 800, 125, 90, pink);
        Label(menu.transform, "A little movement. A little sweetness.", 440, 305, 720, 50, 28);
        Label(menu.transform, "Move your chef. Catch the ingredients.\nMake something lovely, one order at a time.", 430, 374, 740, 85, 25);
        Action(menu.transform, "Start Game", 530, 495, 540, 74, () => Show(levels));
        Action(menu.transform, "How to Play", 530, 586, 540, 65, () => Show(instructions), false);
        Label(menu.transform, "TWO-HAND MOVEMENT GAME  •  KEYBOARD EDITION", 430, 708, 740, 35, 18);
        Label(menu.transform, "A / Left Arrow     •     D / Right Arrow", 430, 810, 740, 40, 23, cream);
    }

    private void BuildLevels()
    {
        levels = Screen("Level Select");
        Panel(levels.transform, "Level backdrop", 65, 35, 1470, 800, cream);
        Header(levels.transform, "Choose your pace", "Three ways to play. Every catch is a small win.");
        string[] names = { "Easy", "Medium", "Hard" };
        string[] descriptions = { "Slow falling speed\nOne ingredient at a time\nRepeating lanes\nNo hazards", "Medium falling speed\nUp to two objects\nRandom positions\nSome hazards", "Fast falling speed\nUp to three objects\nRandom positions\nMore hazards" };
        string[] colors = { "#D9EFCF", "#FFE5B5", "#FFD2DD" };
        for (int i = 0; i < 3; i++)
        {
            DifficultyLevel difficulty = (DifficultyLevel)i;
            float x = 135 + 460 * i;
            Panel(levels.transform, names[i] + " card", x, 310, 410, 365, BakeryArt.Hex(colors[i]));
            Label(levels.transform, names[i], x + 20, 340, 370, 60, 38);
            Label(levels.transform, descriptions[i], x + 25, 424, 360, 160, 25);
            Action(levels.transform, "Select " + names[i], x + 35, 600, 340, 60, () => Begin(difficulty));
        }
        Action(levels.transform, "Back", 650, 735, 300, 60, () => Show(menu), false);
    }

    private void BuildInstructions()
    {
        instructions = Screen("How to Play");
        Panel(instructions.transform, "Instructions card", 170, 55, 1260, 765, cream);
        Header(instructions.transform, "A recipe for a good time", "Move underneath an ingredient. Your tray catches it automatically.");
        Label(instructions.transform, "1   MOVE", 250, 318, 300, 55, 32, pink);
        Label(instructions.transform, "A / Left Arrow: left\nD / Right Arrow: right\nBoth directions: stop", 215, 386, 370, 130, 25);
        Label(instructions.transform, "2   COLLECT", 650, 318, 300, 55, 32, pink);
        Label(instructions.transform, "Follow the order card.\nCatch its three ingredients.\nA fresh recipe follows!", 615, 386, 370, 130, 25);
        Label(instructions.transform, "3   KEEP GOING", 1050, 318, 300, 55, 32, pink);
        Label(instructions.transform, "Avoid dark ! hazards.\nCatches build your combo.\nMisses reset your combo.", 1015, 386, 370, 130, 25);
        Label(instructions.transform, "90 seconds per session  •  Esc to pause  •  No catch button needed", 250, 565, 1100, 65, 25);
        Action(instructions.transform, "Ready to bake", 550, 680, 500, 70, () => Show(levels));
    }

    private void BuildHud()
    {
        hud = Screen("Gameplay HUD");
        Panel(hud.transform, "Top bar", 30, 22, 1540, 95, cream);
        Label(hud.transform, "Bake Catch", 55, 35, 235, 60, 35, pink);
        timeLabel = Label(hud.transform, "01:30", 320, 30, 245, 68, 31);
        scoreLabel = Label(hud.transform, "Score  0", 620, 30, 245, 68, 31);
        comboLabel = Label(hud.transform, "Combo  x0", 920, 30, 260, 68, 29);
        levelLabel = Label(hud.transform, "Easy", 1190, 30, 185, 68, 25);
        Action(hud.transform, "Pause", 1400, 38, 140, 60, () => session.Pause(), false);
        Panel(hud.transform, "Time track", 334, 98, 220, 7, BakeryArt.Hex("#F6DCE0"));
        timeFill = Panel(hud.transform, "Time remaining", 334, 98, 220, 7, pink);
        Panel(hud.transform, "Order", 35, 158, 310, 325, cream);
        Label(hud.transform, "TODAY'S ORDER", 55, 177, 270, 38, 20, pink);
        recipeLabel = Label(hud.transform, "Strawberry Cake", 55, 224, 270, 60, 29);
        orderRows = Label(hud.transform, "", 66, 297, 250, 140, 24, ink, TextAnchor.MiddleLeft);
        Label(hud.transform, "Collect one of each", 55, 441, 270, 27, 18);
        feedback = Label(hud.transform, "", 420, 140, 750, 65, 29, ink);
        Panel(hud.transform, "Left control", 35, 797, 345, 80, BakeryArt.Hex("#FFE0E7"));
        Panel(hud.transform, "Right control", 1220, 797, 345, 80, BakeryArt.Hex("#D8F0FF"));
        leftLabel = Label(hud.transform, "LEFT HAND\nA / Left Arrow", 50, 802, 315, 70, 23);
        rightLabel = Label(hud.transform, "RIGHT HAND\nD / Right Arrow", 1235, 802, 315, 70, 23);
        Label(hud.transform, "Move under ingredients • Catch automatically", 420, 827, 760, 42, 22, cream);
    }

    private void BuildPause()
    {
        pause = Screen("Pause Menu");
        Panel(pause.transform, "Dim", 0, 0, 1600, 900, new Color(0.21f, 0.12f, 0.15f, 0.65f));
        Panel(pause.transform, "Pause card", 505, 170, 590, 560, cream);
        Label(pause.transform, "Take a little break", 540, 220, 520, 70, 40, pink);
        Label(pause.transform, "Your bakery can wait.", 540, 300, 520, 45, 25);
        Action(pause.transform, "Resume", 595, 383, 410, 68, () => session.Resume());
        Action(pause.transform, "Restart", 595, 475, 410, 68, () => Begin(session.Difficulty), false);
        Action(pause.transform, "Main Menu", 595, 567, 410, 68, ReturnMenu, false);
    }

    private void BuildResult()
    {
        result = Screen("Result Screen");
        Panel(result.transform, "Results card", 280, 45, 1040, 805, cream);
        Label(result.transform, "FRESHLY FINISHED", 430, 76, 740, 40, 20, pink);
        Label(result.transform, "Sweet work!", 430, 120, 740, 85, 64, pink);
        Label(result.transform, "Every session is another step forward.", 420, 212, 760, 45, 25);
        Label(result.transform, "Score\nMax Combo\nCatch Count\nMiss Count\nAccuracy\nPlay Time\nLevel\nRecipe Complete", 435, 290, 430, 365, 28, ink, TextAnchor.MiddleLeft);
        resultStats = Label(result.transform, "", 890, 290, 270, 365, 28, ink, TextAnchor.MiddleRight);
        Action(result.transform, "Play Again", 385, 714, 385, 72, () => Begin(session.Difficulty));
        Action(result.transform, "Main Menu", 830, 714, 385, 72, ReturnMenu, false);
    }

    private void Show(GameObject screen)
    {
        foreach (GameObject item in new[] { menu, levels, instructions, hud, pause, result }) item.SetActive(item == screen);
        Button first = screen.GetComponentInChildren<Button>();
        EventSystem.current.SetSelectedGameObject(first != null ? first.gameObject : null);
    }

    private void Begin(DifficultyLevel difficulty) { session.StartSession(difficulty); Show(hud); }
    private void ReturnMenu() { session.MainMenu(); Show(menu); }

    private void Update()
    {
        if (root == null) return;
        if (shownState != session.State)
        {
            shownState = session.State;
            if (shownState == SessionState.Paused) { Show(pause); hud.SetActive(true); }
            else if (shownState == SessionState.Playing) Show(hud);
            else if (shownState == SessionState.Ready) Show(menu);
            else
            {
                Show(result);
                SessionResult r = session.Result;
                resultStats.text = $"{r.score}\n{r.maxCombo}\n{r.catchCount}\n{r.missCount}\n{r.accuracy:0.0}%\n{FormatTime(r.durationSec)}\n{r.level}\n{r.recipeComplete}";
            }
        }
        if (levels.activeSelf && Keyboard.current != null)
        {
            if (Keyboard.current.digit1Key.wasPressedThisFrame) Begin(DifficultyLevel.Easy);
            else if (Keyboard.current.digit2Key.wasPressedThisFrame) Begin(DifficultyLevel.Medium);
            else if (Keyboard.current.digit3Key.wasPressedThisFrame) Begin(DifficultyLevel.Hard);
        }
        if (!hud.activeSelf) return;
        timeLabel.text = "Time  " + FormatTime(session.Remaining);
        scoreLabel.text = "Score  " + session.Statistics.Score;
        comboLabel.text = "Combo  x" + session.Statistics.CurrentCombo;
        levelLabel.text = session.Difficulty.ToString();
        timeFill.rectTransform.sizeDelta = new Vector2(220 * session.Remaining / session.Duration, 7);
        recipeLabel.text = session.Recipe.Current.Name;
        orderRows.text = "";
        for (int i = 0; i < session.Recipe.Current.Ingredients.Length; i++)
            orderRows.text += $"{session.Recipe.Current.Ingredients[i]}   {session.Recipe.Collected[i]}/{session.Recipe.Current.Quantities[i]}\n";
        feedback.text = string.IsNullOrEmpty(session.Feedback) ? session.Statistics.ComboFeedback : session.Feedback + "\n" + session.Statistics.ComboFeedback;
        Keyboard keys = Keyboard.current;
        leftLabel.color = keys != null && (keys.aKey.isPressed || keys.leftArrowKey.isPressed) ? pink : ink;
        rightLabel.color = keys != null && (keys.dKey.isPressed || keys.rightArrowKey.isPressed) ? BakeryArt.Hex("#269CCB") : ink;
    }

    private static string FormatTime(float seconds)
    {
        int whole = Mathf.CeilToInt(Mathf.Max(0, seconds));
        return $"{whole / 60:00}:{whole % 60:00}";
    }
}
