using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour, IListener
{

    public Transform greenBall;
    public Transform redBall;
    public Text gameText;
    Transform winBlock;
    Transform looseBlock1;
    Transform looseBlock2;

    int picks; // Current number of choices made
    static GameManager instance;

    public static GameManager Instance {
        get { return instance; }
    }

    void Awake() {
        if( instance ) {
            DestroyImmediate( gameObject );
            return;     
        } 
        instance = this;
        DontDestroyOnLoad( gameObject );
    }

    void Start() {
        SceneManager.LoadScene( 0 );
    }

    void OnLevelWasLoaded( int level ) {
        gameText = GameObject.FindGameObjectsWithTag( "Text" )[ 0 ].GetComponent<Text>();
        gameText.text = System.String.Join( System.Environment.NewLine,
            new System.String[]
                  {"There are 3 blocks before you.",
                                 "Behind each block is a ball.",
                                 "There are 2 red balls and 1 green ball.",
                                 "Select the block with the green ball and you win.",
                                 "",
                                 "Left-click on a block"
                  } );

        EventManager.Instance.AddListener( EVENT_TYPE.BLOCK_SELECTED, this );

        picks = 0;

        // Pick the block to place the green ball
        System.Random rnd = new System.Random();
        int blockNum = rnd.Next( 0, 3 );

     
        //Place balls
        winBlock = placeBall( greenBall, blockNum );
        looseBlock1 = placeBall( redBall, ( blockNum + 1 ) % 3 );
        looseBlock2 = placeBall( redBall, ( blockNum + 2 ) % 3 );
    }

    // Place ball at desired block
    Transform placeBall( Transform ball, int blockNum ) {
        GameObject block = GameObject.FindGameObjectsWithTag( "Block" + blockNum )[ 0 ];
        Transform ballSpawn = block.transform.GetChild( 0 );
        Instantiate( ball, ballSpawn.position, Quaternion.identity );
        return block.transform;
    }

    public void OnEvent( EVENT_TYPE event_type, Component sender, Object param = null ) {
        switch( event_type ) {
            case EVENT_TYPE.BLOCK_SELECTED:
                OnSelected( sender.tag );
                break;
        }
    }


    void OnSelected( string sender ) {
        picks++;

        if( picks == 1 ) {
            if( sender.Equals( looseBlock1.tag ) ) {
                EventManager.Instance.Notify( EVENT_TYPE.REVEAL_BLOCK, looseBlock2 );
            } else if( sender.Equals( looseBlock2.tag ) ) {
                EventManager.Instance.Notify( EVENT_TYPE.REVEAL_BLOCK, looseBlock1 );
            } else {
                EventManager.Instance.Notify( EVENT_TYPE.REVEAL_BLOCK, looseBlock1 );
            }

            gameText.text = System.String.Join( System.Environment.NewLine,
                new System.String[]
                {
                    "One red ball has been revealed.",
                    "You may choose a different block in light of this new information.",
                    "",
                    "Left-click on a block"
                } );
        }

        if( picks == 2 ) {
            string quitText = "Press esc key to quit\n"
                + "Press r key to restart";

            if( sender.Equals( winBlock.tag ) ) {
                gameText.text = "You Win!!!!\n\n" + quitText;
            } else {
                gameText.text = "You Loose!!!!\n\n" + quitText;
            }

            EventManager.Instance.Notify( EVENT_TYPE.REVEAL_BLOCK, looseBlock1 );
            EventManager.Instance.Notify( EVENT_TYPE.REVEAL_BLOCK, looseBlock2 );
            EventManager.Instance.Notify( EVENT_TYPE.REVEAL_BLOCK, winBlock );
        }
    }

    void Update() {
        if( Input.GetButton( "Cancel" ) ) {
            // Quit Game
            Application.Quit();
        }

        if( Input.GetKey( KeyCode.R ) ) {
            // Restart Game
            EventManager.Instance.RemoveEvent( EVENT_TYPE.REVEAL_BLOCK );
            EventManager.Instance.RemoveEvent( EVENT_TYPE.BLOCK_SELECTED );
            EventManager.Instance.RemoveEvent( EVENT_TYPE.BLOCK_HOVER );
            EventManager.Instance.RemoveEvent( EVENT_TYPE.BLOCK_NOHOVER );
            SceneManager.LoadScene( 0 );
        }
    }
}
