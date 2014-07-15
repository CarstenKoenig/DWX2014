module StateM where

data State s a = State { runState :: s -> (a, s) }

instance Monad (State s) where
  return a = State $ \s -> (a, s)
  (State f) >>= g = 
    State $ \s -> let (a,s') = f s 
                  in runState (g a) s'

(>=>) :: (a -> State s b) -> (b -> State s c) -> (a -> State s c)
(>=>) f g a = f a >>= g