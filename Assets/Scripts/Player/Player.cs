    using System.Collections.Generic;
    using UnityEngine;

    public class Player : SingletonMonobehavior<Player>
    {
        private AnimationOverrides animationOverrides;

        // Movement Parameters
        public float inputX;
        public float inputY;
        public bool isWalking;
        public bool isRunning;
        public bool isIdle;
        public bool isCarrying = false;
        public ToolEffect toolEffect = ToolEffect.none;
        // using tool será para machado, enchada e picareta
        public bool isUsingToolRight;
        public bool isUsingToolLeft;
        public bool isUsingToolUp;
        public bool isUsingToolDown;
        // liftting tool será para regador
        public bool isLiftingToolRight;
        public bool isLiftingToolLeft;
        public bool isLiftingToolUp;
        public bool isLiftingToolDown;
        // picking up creio que será para o cesto
        public bool isPickingUpRight;
        public bool isPickingUpLeft;
        public bool isPickingUpUp;
        public bool isPickingUpDown;
        //swingint tool será para a foice
        public bool isSwingingToolRight;
        public bool isSwingingToolLeft;
        public bool isSwingingToolUp;
        public bool isSwingingToolDown;
        public bool idleUp;
        public bool idleDown;
        public bool idleLeft;
        public bool idleRight;

        private Camera mainCamera;

        private Rigidbody2D rigidBody2D;

        private Direction playerDirection;

        private List<CharacterAttribute> characterAttributeCustomisationList;

        [Tooltip("Should be pupulated in the prefab with the equipped item sprite renderer")]
        [SerializeField] private SpriteRenderer equippedItemSpriteRenderer = null;

        //Player attributes that can be swapped

        private CharacterAttribute armsCharacterAttribute;
        private CharacterAttribute toolCharacterAttribute;

        private float movementSpeed;

        private bool _playerInputIsDisabled = false;

        public bool PlayerInputIsDisabled { get => _playerInputIsDisabled; set => _playerInputIsDisabled = value; }

        protected override void Awake()
        {
            base.Awake();
            rigidBody2D = GetComponent<Rigidbody2D>();

            animationOverrides = GetComponentInChildren<AnimationOverrides>();

            // Initialise swappable character attributes
            armsCharacterAttribute = new CharacterAttribute(CharacterPartAnimator.arms, PartVariantColour.none, PartVariantType.none);

            // Initialise character attribute lis
            characterAttributeCustomisationList = new List<CharacterAttribute>();

            // é um método da unity que irá retornar a câmera principal da cena.
            mainCamera = Camera.main;
        }

        private void Update()
        {
            #region Player Input

            if (!PlayerInputIsDisabled)
            {
                // serve para as animações que ocorrem uma vez só, tipo usar um item. Se a gente não resetar os triggers, elas ficam ativas pra sempre
                ResetAnimationTriggers();

                PlayerMovementInput();

                PlayerWalkInput();

                PlayerTestInput();

            // esse é o gritar do player para o evento
                EventHandler.CallMovementEvent(inputX, inputY,
                    isWalking, isRunning, isIdle, isCarrying,
                    toolEffect,
                    isUsingToolRight, isUsingToolLeft, isUsingToolUp, isUsingToolDown,
                    isLiftingToolRight, isLiftingToolLeft, isLiftingToolUp, isLiftingToolDown,
                    isPickingUpRight, isPickingUpLeft, isPickingUpUp, isPickingUpDown,
                    isSwingingToolRight, isSwingingToolLeft, isSwingingToolUp, isSwingingToolDown,
                    false, false, false, false);

            }

           

            #endregion
        }

        private void FixedUpdate()
        {
            PlayerMovement();
        
        }

        private void PlayerMovement()
        {
            Vector2 move = new Vector2(inputX * movementSpeed * Time.deltaTime, inputY * movementSpeed * Time.deltaTime);

            rigidBody2D.MovePosition(rigidBody2D.position + move);
        }


        private void ResetAnimationTriggers()
        {
            isPickingUpRight = false;
            isPickingUpLeft = false;
            isPickingUpUp = false;
            isPickingUpDown = false;
            isUsingToolRight = false;
            isUsingToolLeft = false;
            isUsingToolUp = false;
            isUsingToolDown = false;
            isLiftingToolRight = false;
            isLiftingToolLeft = false;
            isLiftingToolUp = false;
            isLiftingToolDown = false;
            isSwingingToolRight = false;
            isSwingingToolLeft = false;
            isSwingingToolUp = false;
            isSwingingToolDown = false;
            toolEffect = ToolEffect.none;
        }
    

        // serve só para definir algumas coisas, não move de fato
        private void PlayerMovementInput()
        {
            inputY = Input.GetAxisRaw("Vertical");
            inputX = Input.GetAxisRaw("Horizontal");

            // Ajuste para movimento diagonal (normalização manual)
            if (inputY != 0 && inputX != 0)
            {
                inputX = inputX * 0.71f;
                inputY = inputY * 0.71f;
            }


            if (inputX != 0 || inputY != 0)
            {
                isRunning = true;
                isWalking = false;
                isIdle = false;
                movementSpeed = Settings.runningSpeed;

                // Captura a direção do jogador para o sistema de save
                if (inputX < 0)
                {
                    playerDirection = Direction.left;
                }
                else if (inputX > 0)
                {
                    playerDirection = Direction.right;
                }
                else if (inputY < 0)
                {
                    playerDirection = Direction.down;
                }
                else
                {
                    playerDirection = Direction.up;
                }
            }
            else if (inputX == 0 && inputY == 0)
            {
                isRunning = false;
                isWalking = false;
                isIdle = true;
            }
        }

        
        // serve só pra mudar a velocidade msm
        private void PlayerWalkInput()
        {
            // Verifica se a tecla Shift está pressionada para alternar entre andar e correr
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                isRunning = false;
                isWalking = true;
                isIdle = false;
                movementSpeed = Settings.walkingSpeed;
            }
            else
            {
                isRunning = true;
                isWalking = false;
                isIdle = false;
                movementSpeed = Settings.runningSpeed;
            }
        }

        // TODO: Remove
        /// <summary>
        /// Temp routine for test input
        /// </summary>
        private void PlayerTestInput()
        {
            // Trigger Advance Time
            if (Input.GetKey(KeyCode.T))
            {
                TimeManager.Instance.TestAdvanceGameMinute();
            }

            // Trigger Advance Day
            if (Input.GetKeyDown(KeyCode.G))
            {
                TimeManager.Instance.TestAdvanceGameDay();
            }
        }



        public void EnablePlayerInput()
        {
            PlayerInputIsDisabled = false;
        } 

        public void DisablePlayerInput()
        {
            PlayerInputIsDisabled = true;
        }

        private void ResetMovement()
        {
            inputX = 0f;
            inputY = 0f;
            isRunning = false;
            isWalking = false;
            isIdle = true;
        }
        public void DisablePlayerInputAndResetMovement()
        {
            DisablePlayerInput();
            ResetMovement();
            EventHandler.CallMovementEvent(inputX, inputY,
                    isWalking, isRunning, isIdle, isCarrying,
                    toolEffect,
                    isUsingToolRight, isUsingToolLeft, isUsingToolUp, isUsingToolDown,
                    isLiftingToolRight, isLiftingToolLeft, isLiftingToolUp, isLiftingToolDown,
                    isPickingUpRight, isPickingUpLeft, isPickingUpUp, isPickingUpDown,
                    isSwingingToolRight, isSwingingToolLeft, isSwingingToolUp, isSwingingToolDown,
                    false, false, false, false);
        }

        public Vector3 GetPlayerViewportPosition()
        {
            return mainCamera.WorldToViewportPoint(transform.position);
        }


        public void ClearCarriedItem()
        {
            equippedItemSpriteRenderer.sprite = null;
            equippedItemSpriteRenderer.color = new Color(1f, 1f, 1f, 0f);

            // Apply base character arms customisation
            armsCharacterAttribute.partVariantType = PartVariantType.none;
            //esse abaixo eu que criei com o GPT
            armsCharacterAttribute.partVariantColour = PartVariantColour.none;
            characterAttributeCustomisationList.Clear();
            characterAttributeCustomisationList.Add(armsCharacterAttribute);
            animationOverrides.ApplyCharacterCustomisationParameters(characterAttributeCustomisationList);

            isCarrying = false;
        }
        public void ShowCarriedItem(int itemCode)
        {
            ItemsData itemsData = InventoryManager.Instance.GetItemData(itemCode);

            if (itemsData != null)
            {
                equippedItemSpriteRenderer.sprite = itemsData.itemSprite;
                equippedItemSpriteRenderer.color = new Color(1f, 1f, 1f, 1f);

                // Apply 'carry' character arms customisation
                armsCharacterAttribute.partVariantType = PartVariantType.carry;
                characterAttributeCustomisationList.Clear();
                characterAttributeCustomisationList.Add(armsCharacterAttribute);
                animationOverrides.ApplyCharacterCustomisationParameters(characterAttributeCustomisationList);

                isCarrying = true;
            }
        }
    }
