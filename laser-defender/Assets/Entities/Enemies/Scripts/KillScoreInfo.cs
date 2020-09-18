using UnityEngine;
using System.Collections;

public class KillScoreInfo : MonoBehaviour {

	[SerializeField] private TextMesh _textLabel = null;
	[SerializeField] private Animator _animator = null;

	private readonly int _showScoreHash = Animator.StringToHash("ShowKillScore");

	public void ShowScore(string scoreText)
	{
		_textLabel.text = scoreText;
		_animator.Play(_showScoreHash);
	}
	
	public void KillScoreGO()
	{
		Destroy(this.gameObject);
	}
	
}
