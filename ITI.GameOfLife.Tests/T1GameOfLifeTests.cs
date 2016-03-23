using System;
using NUnit.Framework;

namespace ITI.GameOfLife.Tests
{
    [TestFixture]
    public class T1GameOfLifeTests
    {
        [TestCase( 10, 5 )]
        [TestCase( 20, 30 )]
        public void T01_ctor_initializes_the_game_correctly( int width, int height )
        {
            Game sut = new Game( width, height );

            Assert.That( sut.Width, Is.EqualTo( width ) );
            Assert.That( sut.Height, Is.EqualTo( height ) );
        }

        [TestCase( -1, 3 )]
        [TestCase( 0, 3 )]
        [TestCase( 5, -1 )]
        [TestCase( 5, 0 )]
        public void T02_ctor_with_invalid_args_should_throw_ArgumentException( int width, int height )
        {
            Assert.Throws<ArgumentException>( () => new Game( width, height ) );
        }

        [Test]
        public void T03_cells_are_dead_by_default()
        {
            const int width = 20;
            const int height = 15;
            Game sut = new Game( width, height );

            for( int i = 0; i < width; i++ )
            {
                for( int j = 0; j < height; j++ ) Assert.That( sut.IsAlive( i, j ), Is.False );
            }
        }

        [Test]
        public void T04_can_give_life()
        {
            Game sut = new Game( 50, 100 );
            sut.GiveLive( 10, 80 );
            Assert.That( sut.IsAlive( 10, 80 ), Is.True );
        }

        [Test]
        public void T05_can_kill_cell()
        {
            Game sut = new Game( 100, 100 );
            sut.GiveLive( 10, 80 );

            sut.Kill( 10, 80 );

            Assert.That( sut.IsAlive( 10, 80 ), Is.False );
        }


        [TestCase( -1, 10 )]
        [TestCase( 5, -1 )]
        [TestCase( 10, 10 )]
        [TestCase( 7, 20 )]
        public void T06_GiveLive_Kill_and_IsAlive_with_invalid_args_should_throw_ArgumentException( int x, int y )
        {
            Game sut = new Game( 10, 20 );
            Assert.Throws<ArgumentException>( () => sut.GiveLive( x, y ) );
            Assert.Throws<ArgumentException>( () => sut.Kill( x, y ) );
            Assert.Throws<ArgumentException>( () => sut.IsAlive( x, y ) );
        }

        [Test]
        public void T07_NextTurn_when_all_cells_are_dead_shouldnt_change_game_state()
        {
            Game sut = new Game( 30, 20 );
            sut.NextTurn();
            for( int i = 0; i < sut.Width; i++ )
            {
                for( int j = 0; j < sut.Height; j++ ) Assert.That( sut.IsAlive( i, j ), Is.False );
            }
        }

        // +-+-+-+-+-+-+              +-+-+-+-+-+-+
        // | | | | | | |              | | | |x| | |
        // +-+-+-+-+-+-+              +-+-+-+-+-+-+
        // | | |x|x|x| |              | | |x| |x| |
        // +-+-+-+-+-+-+              +-+-+-+-+-+-+
        // | | |x|x|x| |  =========>  | | | | |x| |
        // +-+-+-+-+-+-+              +-+-+-+-+-+-+
        // | |x|x| | | |              | |x|x| | | |
        // +-+-+-+-+-+-+              +-+-+-+-+-+-+
        // | | | | | | |              | | | | | | |
        // +-+-+-+-+-+-+              +-+-+-+-+-+-+
        [Test]
        public void T08_NextTurn_should_change_state_correctly()
        {
            Game sut = new Game( 6, 5 );
            sut.GiveLive( 2, 1 );
            sut.GiveLive( 3, 1 );
            sut.GiveLive( 4, 1 );
            sut.GiveLive( 2, 2 );
            sut.GiveLive( 3, 2 );
            sut.GiveLive( 4, 2 );
            sut.GiveLive( 1, 3 );
            sut.GiveLive( 2, 3 );

            sut.NextTurn();

            Assert.That( sut.IsAlive( 0, 0 ), Is.False );
            Assert.That( sut.IsAlive( 1, 0 ), Is.False );
            Assert.That( sut.IsAlive( 2, 0 ), Is.False );
            Assert.That( sut.IsAlive( 3, 0 ), Is.True );
            Assert.That( sut.IsAlive( 4, 0 ), Is.False );
            Assert.That( sut.IsAlive( 5, 0 ), Is.False );
            Assert.That( sut.IsAlive( 0, 1 ), Is.False );
            Assert.That( sut.IsAlive( 1, 1 ), Is.False );
            Assert.That( sut.IsAlive( 2, 1 ), Is.True );
            Assert.That( sut.IsAlive( 3, 1 ), Is.False );
            Assert.That( sut.IsAlive( 4, 1 ), Is.True );
            Assert.That( sut.IsAlive( 5, 1 ), Is.False );
            Assert.That( sut.IsAlive( 0, 2 ), Is.False );
            Assert.That( sut.IsAlive( 1, 2 ), Is.False );
            Assert.That( sut.IsAlive( 2, 2 ), Is.False );
            Assert.That( sut.IsAlive( 3, 2 ), Is.False );
            Assert.That( sut.IsAlive( 4, 2 ), Is.True );
            Assert.That( sut.IsAlive( 5, 2 ), Is.False );
            Assert.That( sut.IsAlive( 0, 3 ), Is.False );
            Assert.That( sut.IsAlive( 1, 3 ), Is.True );
            Assert.That( sut.IsAlive( 2, 3 ), Is.True );
            Assert.That( sut.IsAlive( 3, 3 ), Is.False );
            Assert.That( sut.IsAlive( 4, 3 ), Is.False );
            Assert.That( sut.IsAlive( 5, 3 ), Is.False );
            Assert.That( sut.IsAlive( 0, 4 ), Is.False );
            Assert.That( sut.IsAlive( 1, 4 ), Is.False );
            Assert.That( sut.IsAlive( 2, 4 ), Is.False );
            Assert.That( sut.IsAlive( 3, 4 ), Is.False );
            Assert.That( sut.IsAlive( 4, 4 ), Is.False );
            Assert.That( sut.IsAlive( 5, 4 ), Is.False );
        }

        // +-+-+-+-+-+-+              +-+-+-+-+-+-+
        // |x| |x| |x| |              |x|x|x|x|x|x|
        // +-+-+-+-+-+-+              +-+-+-+-+-+-+
        // | | | | | | |  =========>  |x|x|x|x|x|x|
        // +-+-+-+-+-+-+              +-+-+-+-+-+-+
        // | |x| |x| |x|              |x|x|x|x|x|x|
        // +-+-+-+-+-+-+              +-+-+-+-+-+-+
        [Test]
        public void T09_game_board_is_toric()
        {
            Game sut = new Game( 6, 3 );
            sut.GiveLive( 0, 0 );
            sut.GiveLive( 2, 0 );
            sut.GiveLive( 4, 0 );
            sut.GiveLive( 1, 2 );
            sut.GiveLive( 3, 2 );
            sut.GiveLive( 5, 2 );

            sut.NextTurn();

            for( int i = 0; i < sut.Width; i++ )
            {
                for( int j = 0; j < sut.Height; j++ ) Assert.That( sut.IsAlive( i, j ), Is.True );
            }
        }

        // +-+-+-+-+              +-+-+-+-+              +-+-+-+-+
        // | | | | |              | | | | |              | | | | |
        // +-+-+-+-+              +-+-+-+-+              +-+-+-+-+
        // | |x| | |  =========>  | | | | |  =========>  | | | | |
        // +-+-+-+-+              +-+-+-+-+              +-+-+-+-+
        // | | | | |              | | | | |              | | | | |
        // +-+-+-+-+              +-+-+-+-+              +-+-+-+-+
        [Test]
        public void T10_NextTurn_should_return_true_if_state_changed_otherwise_should_return_false()
        {
            Game sut = new Game( 4, 3 );
            sut.GiveLive( 1, 1 );

            Assert.That( sut.NextTurn(), Is.True );
            Assert.That( sut.NextTurn(), Is.False );
        }
    }
}
